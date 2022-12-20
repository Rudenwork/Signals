using MassTransit;
using Signals.App.Core.Stage;
using Signals.App.Database;
using Signals.App.Database.Entities.Stages;
using Signals.App.Extensions;

namespace Signals.App.Core.Execution
{
    public class Next
    {
        public class Message
        {
            public Guid ExecutionId { get; set; }
        }

        public class Consumer : IConsumer<Message>
        {
            private ILogger<Consumer> Logger { get; }
            private SignalsContext SignalsContext { get; }

            public Consumer(ILogger<Consumer> logger, SignalsContext signalsContext)
            {
                Logger = logger;
                SignalsContext = signalsContext;
            }

            public async Task Consume(ConsumeContext<Message> context)
            {
                context.EnsureFresh();

                Logger.LogInformation($"[{context.Message.ExecutionId}] Selecting Next Stage");

                var execution = SignalsContext.Executions.Find(context.Message.ExecutionId);

                if (execution is null)
                    return;

                var stage = execution.StageId switch
                {
                    null => SignalsContext.Stages.First(x => x.SignalId == execution.SignalId && x.PreviousStageId == null),
                    _ => SignalsContext.Stages.FirstOrDefault(x => x.PreviousStageId == execution.StageId)
                };

                execution.StageId = stage?.Id;

                SignalsContext.Update(execution);
                SignalsContext.SaveChanges();

                object message = stage switch
                {
                    WaitingStageEntity => new ExecuteWaiting.Message { ExecutionId = execution.Id, StageId = stage.Id },
                    ConditionStageEntity => new ExecuteCondition.Message { ExecutionId = execution.Id, StageId = stage.Id },
                    NotificationStageEntity => new ExecuteNotification.Message { ExecutionId = execution.Id, StageId = stage.Id },
                    _ => new Stop.Message { ExecutionId = execution.Id }
                };

                await context.Publish(message);
            }
        }

        public class FaultConsumer : IConsumer<Fault<Message>>
        {
            public async Task Consume(ConsumeContext<Fault<Message>> context)
            {
                await context.Publish(new Stop.Message { ExecutionId = context.Message.Message.ExecutionId });
            }
        }
    }
}
