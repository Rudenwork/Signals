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

                var stage = SignalsContext.Stages.Find(execution.StageId);
                var stagesQuery = SignalsContext.Stages.Where(x => x.SignalId == execution.SignalId);

                var nextStage = stage switch
                {
                    null => stagesQuery.First(x => x.Index == 0),
                    _ => stagesQuery.OrderBy(x => x.Index).FirstOrDefault(x => x.Index > stage.Index)
                };

                execution.StageId = nextStage?.Id;

                SignalsContext.Update(execution);
                SignalsContext.SaveChanges();

                object message = nextStage switch
                {
                    WaitingStageEntity waiting => new ExecuteWaiting.Message { ExecutionId = execution.Id, Stage = waiting },
                    ConditionStageEntity condition => new ExecuteCondition.Message { ExecutionId = execution.Id, Stage = condition },
                    NotificationStageEntity notification => new ExecuteNotification.Message { ExecutionId = execution.Id, Stage = notification },
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
