using MassTransit;
using MassTransit.Mediator;
using Signals.App.Core.Block;
using Signals.App.Core.Execution;
using Signals.App.Database;
using Signals.App.Database.Entities.Stages;
using Signals.App.Extensions;
using Signals.App.Services;
using System;

namespace Signals.App.Core.Stage
{
    public class ExecuteCondition
    {
        public class Message
        {
            public Guid StageId { get; set; }
            public Guid ExecutionId { get; set; }
            public int RetryAttempt { get; set; }
        }

        public class Consumer : IConsumer<Message>
        {
            private ILogger<Consumer> Logger { get; }
            private SignalsContext SignalsContext { get; }
            private IMediator Mediator { get; }
            private Scheduler Scheduler { get; }

            public Consumer(ILogger<Consumer> logger, SignalsContext signalsContext, IMediator mediator, Scheduler scheduler)
            {
                Logger = logger;
                SignalsContext = signalsContext;
                Mediator = mediator;
                Scheduler = scheduler;
            }

            public async Task Consume(ConsumeContext<Message> context)
            {
                context.EnsureFresh();
                var message = context.Message;

                Logger.LogInformation($"[{message.ExecutionId}] Executing Condition Stage [{message.StageId}] - Retry Attempt [{message.RetryAttempt}]");

                if (SignalsContext.Executions.Find(message.ExecutionId) is null)
                    return;

                var stage = SignalsContext.Stages.Find(message.StageId) as ConditionStageEntity;

                var block = SignalsContext.Blocks
                    .Where(x => x.StageId == message.StageId && x.ParentBlockId == null)
                    .FirstOrDefault();

                var response = await Mediator.SendRequest(new EvaluateBlock.Request { BlockId = block.Id });

                if (response.Result) 
                {
                    await context.Publish(new Next.Message { ExecutionId = message.ExecutionId });
                }
                else if (message.RetryAttempt < stage.RetryCount)
                {
                    message.RetryAttempt++;
                    await Scheduler.Publish(message, DateTime.UtcNow + (stage.RetryDelay ?? TimeSpan.Zero), message.ExecutionId);
                }
                else
                {
                    await context.Publish(new Stop.Message { ExecutionId = message.ExecutionId });
                }
            }
        }

        public class FaultConsumer : IConsumer<Fault<Message>>
        {
            private SignalsContext SignalsContext { get; }
            private Scheduler Scheduler { get; }

            public FaultConsumer(SignalsContext signalsContext, Scheduler scheduler)
            {
                SignalsContext = signalsContext;
                Scheduler = scheduler;
            }

            public async Task Consume(ConsumeContext<Fault<Message>> context)
            {
                var message = context.Message.Message;
                var stage = SignalsContext.Stages.Find(message.StageId) as ConditionStageEntity;

                if (message.RetryAttempt < stage.RetryCount)
                {
                    message.RetryAttempt++;
                    await Scheduler.Publish(message, DateTime.UtcNow + (stage.RetryDelay ?? TimeSpan.Zero), message.ExecutionId);
                }
                else
                {
                    await context.Publish(new Stop.Message { ExecutionId = message.ExecutionId });
                }
            }
        }
    }
}
