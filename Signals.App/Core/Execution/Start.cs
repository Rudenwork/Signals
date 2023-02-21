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
            private IBus Bus { get; }

            public Consumer(ILogger<Consumer> logger, SignalsContext signalsContext, IBus bus)
            {
                Logger = logger;
                SignalsContext = signalsContext;
                Bus = bus;
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

                await Bus.Publish(new Next.Message { ExecutionId = executionId });
            }
        }

        public class FaultConsumer : IConsumer<Fault<Message>>
        {
            private SignalsContext SignalsContext { get; }

            public FaultConsumer(SignalsContext signalsContext)
            {
                SignalsContext = signalsContext;
            }

            public async Task Consume(ConsumeContext<Fault<Message>> context)
            {
                var execution = SignalsContext.Executions.FirstOrDefault(x => x.SignalId == context.Message.Message.SignalId);

                if (execution is not null)
                {
                    await context.Publish(new Stop.Message { ExecutionId = execution.Id });
                }
            }
        }
    }
}
