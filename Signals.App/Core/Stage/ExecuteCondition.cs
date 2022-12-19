using MassTransit;
using Signals.App.Core.Execution;
using Signals.App.Database;
using Signals.App.Extensions;

namespace Signals.App.Core.Stage
{
    public class ExecuteCondition
    {
        public class Message
        {
            public Guid StageId { get; set; }
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

                var stageId = context.Message.StageId;
                var executionId = context.Message.ExecutionId;

                Logger.LogInformation($"[{executionId}] Condition Stage {stageId}");
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
