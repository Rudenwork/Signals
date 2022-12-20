using MassTransit;
using MassTransit.Mediator;
using Signals.App.Database.Entities;
using Signals.App.Database.Entities.Blocks;

namespace Signals.App.Core.Block
{
    public class EvaluateBlock
    {
        public class Request : Request<Response>
        {
            public BlockEntity Block { get; set; }
        }

        public class Response
        {
            public bool Result { get; set; }
        }

        public class Consumer : IConsumer<Request>
        {
            private IMediator Mediator { get; }

            public Consumer(IMediator mediator)
            {
                Mediator = mediator;
            }

            public async Task Consume(ConsumeContext<Request> context)
            {
                var response = context.Message.Block switch
                {
                    GroupBlockEntity block => await Mediator.SendRequest(new EvaluateGroupBlock.Request { Block = block }),
                    ValueBlockEntity block => await Mediator.SendRequest(new EvaluateValueBlock.Request { Block = block }),
                    ChangeBlockEntity block => await Mediator.SendRequest(new EvaluateChangeBlock.Request { Block = block })
                };

                await context.RespondAsync(response);
            }
        }
    }
}
