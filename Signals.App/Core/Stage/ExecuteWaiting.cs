using MassTransit;
using Signals.App.Core.Execution;
using Signals.App.Database;
using Signals.App.Extensions;
using Signals.App.Services;

namespace Signals.App.Core.Stage
{
    public class ExecuteWaiting
    {
        public class Message
        {
            public Guid ExecutionId { get; set; }
            public Guid StageId { get; set; }
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
                context.EnsureFresh();

                Logger.LogInformation($"[{context.Message.ExecutionId}] Executing Waiting Stage {context.Message.StageId}");

                var execution = SignalsContext.Executions.Find(context.Message.ExecutionId);

                if (execution is null)
                    return;

                var stage = SignalsContext.WaitingStages.Find(context.Message.StageId);

                await Scheduler.Publish(new Next.Message { ExecutionId = execution.Id }, DateTime.UtcNow + stage.Period, execution.Id);
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
