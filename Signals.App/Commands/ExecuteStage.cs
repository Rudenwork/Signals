using MediatR;
using Microsoft.EntityFrameworkCore;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Services;

namespace Signals.App.Commands
{
    public class ExecuteStage
    {
        public class Command : IRequest
        {
            public Guid SignalId { get; set; }
        }

        private class Handler : IRequestHandler<Command>
        {
            private SignalsContext SignalsContext { get; }
            private CommandService CommandService { get; }

            public Handler(SignalsContext signalsContext, CommandService commandService)
            {
                SignalsContext = signalsContext;
                CommandService = commandService;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var signalExecution = SignalsContext.SignalExecutions.FirstOrDefault(x => x.SignalId == command.SignalId);

                if (signalExecution is null)
                    return Unit.Value;

                var stage = SignalsContext.Stages
                    .Where(x => x.Id == signalExecution.StageId)
                    .Include(x => x.Parameters)
                    .FirstOrDefault();

                var parameters = stage.Parameters.ToDictionary(x => x.Key, x => x.Value);

                switch (stage.Type)
                {
                    case StageEntity.StageType.Condition:
                        return await CommandService.Execute(new ExecuteConditionStage.Command 
                        { 
                            SignalId = stage.SignalId,
                            RetryAttempt = signalExecution.StageRetryAttempt,
                            RetryCount = int.Parse(parameters[StageParameterEntity.ParameterKey.RetryCount]),
                            RetryDelay = TimeSpan.Parse(parameters[StageParameterEntity.ParameterKey.RetryDelay])
                        });
                    case StageEntity.StageType.Waiting:
                        return await CommandService.Execute(new ExecuteWaitingStage.Command
                        {
                            SignalId = stage.SignalId,
                            IsBeginning = signalExecution.StageRetryAttempt is 0,
                            Period = TimeSpan.Parse(parameters[StageParameterEntity.ParameterKey.Period])
                        });
                    case StageEntity.StageType.Notification:
                        return await CommandService.Execute(new ExecuteNotificationStage.Command 
                        { 
                            SignalId = stage.SignalId
                        });
                    default:
                        return Unit.Value;
                }
            }
        }
    }
}
