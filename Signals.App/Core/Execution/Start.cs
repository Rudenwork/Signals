using MassTransit;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Extensions;

namespace Signals.App.Core.Execution
{
    public class Start
    {
        public class Message
        {
            public Guid SignalId { get; set; }
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

                var signalId = context.Message.SignalId;
                var executionId = Guid.NewGuid();

                Logger.LogInformation($"[{executionId}] Starting Signal {signalId}");

                if (SignalsContext.Executions.Any(x => x.SignalId == signalId))
                {
                    return;
                }

                SignalsContext.Executions.Add(new ExecutionEntity
                {
                    Id = executionId,
                    SignalId = signalId
                });

                SignalsContext.SaveChanges();

                await context.Publish(new Next.Message { ExecutionId = executionId });
            }
        }
    }
}
