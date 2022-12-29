using MassTransit;
using Signals.App.Core.Execution;
using Signals.App.Database;
using Signals.App.Database.Entities.Stages;
using Signals.App.Extensions;

namespace Signals.App.Core.Stage
{
    public class ExecuteNotification
    {
        public class Message
        {
            public Guid ExecutionId { get; set; }
            public NotificationStageEntity Stage { get; set; }
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

                var executionId = context.Message.ExecutionId;

                Logger.LogInformation($"[{executionId}] Notification Stage {context.Message.Stage.Id}");
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
