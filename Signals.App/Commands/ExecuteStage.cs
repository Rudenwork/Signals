using MediatR;
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

                var stage = SignalsContext.Stages.Find(signalExecution.StageId);

                var stageParameters = SignalsContext.StageParameters
                    .Where(x => x.StageId == stage.Id)
                    .ToDictionary(x => x.Key, x => x.Value);

                if (stage.Type == StageEntity.StageType.Condition)
                {
                    ///TODO: Condition Stage Logic

                    if (false/*condition result is false*/)
                    {
                        var retryCount = int.Parse(stageParameters[StageParameterEntity.ParameterKey.RetryCount]);

                        if (retryCount > signalExecution.StageRetryAttempt)
                        {
                            var scheduledOn = DateTime.UtcNow + TimeSpan.Parse(stageParameters[StageParameterEntity.ParameterKey.RetryDelay]);
                            await CommandService.Execute(new RescheduleStage.Command { SignalId = stage.SignalId, ScheduleOn = scheduledOn });

                            return Unit.Value;
                        }

                        await CommandService.Execute(new StopSignal.Command { SignalId = stage.SignalId });

                        return Unit.Value;
                    }
                }
                else if (stage.Type == StageEntity.StageType.Waiting && signalExecution.StageRetryAttempt is 0)
                {
                    var scheduledOn = DateTime.UtcNow + TimeSpan.Parse(stageParameters[StageParameterEntity.ParameterKey.Period]);
                    await CommandService.Execute(new RescheduleStage.Command { SignalId = stage.SignalId, ScheduleOn = scheduledOn });

                    return Unit.Value;
                }
                else if (stage.Type == StageEntity.StageType.Notification)
                {
                    ///TODO: Notification Stage Logic
                }

                await CommandService.Execute(new NextStage.Command { SignalId = stage.SignalId });

                return Unit.Value;
            }
        }
    }
}
