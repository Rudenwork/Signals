using MassTransit;
using MassTransit.Mediator;
using Signals.App.Database;
using Signals.App.Database.Entities.Blocks;

namespace Signals.App.Core.Block
{
    public class EvaluateChangeBlock
    {
        public class Request : Request<EvaluateBlock.Response>
        {
            public ChangeBlockEntity Block { get; set; }
        }

        public class Consumer : IConsumer<Request>
        {
            private ILogger<Consumer> Logger { get; }
            private SignalsContext SignalsContext { get; }
            private IMediator Mediator { get; }

            public Consumer(ILogger<Consumer> logger, SignalsContext signalsContext, IMediator mediator)
            {
                Logger = logger;
                SignalsContext = signalsContext;
                Mediator = mediator;
            }

            public async Task Consume(ConsumeContext<Request> context)
            {
                Logger.LogInformation($"Evaluating Change Block");

                await context.RespondAsync(new EvaluateBlock.Response { Result = true });
            }
        }
    }
}
