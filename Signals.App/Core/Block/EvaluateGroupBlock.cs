using MassTransit;
using MassTransit.Mediator;
using Signals.App.Database;
using Signals.App.Database.Entities.Blocks;

namespace Signals.App.Core.Block
{
    public class EvaluateGroupBlock
    {
        public class Request : Request<EvaluateBlock.Response>
        {
            public GroupBlockEntity Block { get; set; }
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
                var block = context.Message.Block;

                Logger.LogInformation($"Evaluating Group Block");

                var isAnd = block.Type == GroupBlockType.And;

                var children = SignalsContext.Blocks
                    .Where(x => x.ParentBlockId == block.Id)
                    .ToList();

                foreach (var child in children)
                {
                    var response = await Mediator.SendRequest(new EvaluateBlock.Request { Block = child });
                    var isBlockSucceded = response.Result;
                    
                    if (isAnd)
                    {
                        if (!isBlockSucceded)
                        {
                            await context.RespondAsync(new EvaluateBlock.Response { Result = false });
                            return;
                        }
                    }
                    else
                    {
                        if (isBlockSucceded)
                        {
                            await context.RespondAsync(new EvaluateBlock.Response { Result = true });
                            return;
                        }
                    }
                }

                await context.RespondAsync(new EvaluateBlock.Response { Result = isAnd });

            }
        }
    }
}
