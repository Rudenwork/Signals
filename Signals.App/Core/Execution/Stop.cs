using MassTransit;
using Signals.App.Database;
using Signals.App.Services;

namespace Signals.App.Core.Execution
{
    public class Stop
    {
        public class Message
        {
            public Guid ExecutionId { get; set; }
        }

        public class Consumer : IConsumer<Message>
        {
            private ILogger<Consumer> Logger { get; }
            private SignalsContext SignalsContext { get; }
            private Scheduler Scheduler { get; }

            public Consumer(ILogger<Consumer> logger, SignalsContext signalsContext, Scheduler scheduler)
            {
                Logger = logger;
                SignalsContext = signalsContext;
                Scheduler = scheduler;
            }

            public async Task Consume(ConsumeContext<Message> context)
            {
                Logger.LogInformation($"[{context.Message.ExecutionId}] Stopping Signal");

                var execution = SignalsContext.Executions.Find(context.Message.ExecutionId);

                if (execution is null)
                    return;

                SignalsContext.Remove(execution);
                SignalsContext.SaveChanges();

                await Scheduler.CancelPublish(execution.Id);
            }
        }
    }
}
