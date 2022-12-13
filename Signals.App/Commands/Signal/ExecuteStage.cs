using MediatR;
using Signals.App.Commands.Stage;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Services;

namespace Signals.App.Commands.Signal
{
    public class ExecuteStage
    {
        public class Command : IRequest<Unit>
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

                switch (stage.Type)
                {
                    case StageEntity.StageType.Condition:
                        var conditionStage = SignalsContext.ConditionStages.Find(stage.Id);
                        return await CommandService.Execute(new ExecuteConditionStage.Command
                        {
                            SignalId = conditionStage.SignalId,
                            StageId = conditionStage.Id,
                            RetryAttempt = signalExecution.StageRetryAttempt,
                            RetryCount = conditionStage.RetryCount ?? 0,
                            RetryDelay = conditionStage.RetryDelay ?? TimeSpan.Zero
                        });
                    case StageEntity.StageType.Waiting:
                        var waitingStage = SignalsContext.WaitingStages.Find(stage.Id);
                        return await CommandService.Execute(new ExecuteWaitingStage.Command
                        {
                            SignalId = waitingStage.SignalId,
                            IsBeginning = signalExecution.StageRetryAttempt is 0,
                            Period = waitingStage.Period
                        });
                    case StageEntity.StageType.Notification:
                        var notificationStage = SignalsContext.NotificationStages.Find(stage.Id);
                        return await CommandService.Execute(new ExecuteNotificationStage.Command
                        {
                            SignalId = notificationStage.SignalId
                        });
                    default:
                        return Unit.Value;
                }
            }
        }
    }
}
