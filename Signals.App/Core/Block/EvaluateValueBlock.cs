using MassTransit;
using MassTransit.Mediator;
using Signals.App.Core.Indicators;
using Signals.App.Database;
using Signals.App.Database.Entities.Blocks;

namespace Signals.App.Core.Block
{
    public class EvaluateValueBlock
    {
        public class Request : Request<EvaluateBlock.Response>
        {
            public ValueBlockEntity Block { get; set; }
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
                Logger.LogInformation($"Evaluating Value Block");

                var leftIndicator = SignalsContext.Indicators.Find(context.Message.Block.LeftIndicatorId);
                var rightIndicator = SignalsContext.Indicators.Find(context.Message.Block.RightIndicatorId);

                var leftResponse = await Mediator.SendRequest(new CalculateIndicator.Request { Indicator = leftIndicator });
                var rightResponse = await Mediator.SendRequest(new CalculateIndicator.Request { Indicator = rightIndicator });

                var leftResult = leftResponse.Result;
                var rightResult = rightResponse.Result;

                var result = context.Message.Block.Operator switch
                {
                    ValueBlockOperator.LessOrEqual => leftResult <= rightResult,
                    ValueBlockOperator.GreaterOrEqual => leftResult >= rightResult
                };

                await context.RespondAsync(new EvaluateBlock.Response { Result = result });
            }
        }
    }
}
