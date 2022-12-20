using MassTransit;
using MassTransit.Mediator;
using Signals.App.Database;

namespace Signals.App.Core.Block
{
    public class EvaluateBlock
    {
        public class Request : Request<Response>
        {
            public Guid BlockId { get; set; }
        }

        public class Response
        {
            public bool Result { get; set; }
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
                await context.RespondAsync(new Response { Result = false });
            }
        }
    }
}
