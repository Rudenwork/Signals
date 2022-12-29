using MassTransit;
using MassTransit.Mediator;
using Signals.App.Core.Indicators;
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

                var block = context.Message.Block;

                var indicator = SignalsContext.Indicators.Find(block.IndicatorId);

                var oldResponse = await Mediator.SendRequest(new CalculateIndicator.Request 
                { 
                    Indicator = indicator, 
                    Time = DateTime.UtcNow - block.Period 
                });

                var newResponse = await Mediator.SendRequest(new CalculateIndicator.Request { Indicator = indicator });

                var oldResult = oldResponse.Result;
                var newResult = newResponse.Result;

                var diff = newResult - oldResult;

                if (block.IsPercentage) 
                {
                    diff = (diff / oldResult) * 100;
                }

                var result = block.Type switch
                {
                    ChangeBlockType.Increase => block.Operator switch
                    {
                        ChangeBlockOperator.LessOrEqual => diff > 0 && Math.Abs(diff) <= block.Target,
                        ChangeBlockOperator.GreaterOrEqual => diff > 0 && Math.Abs(diff) >= block.Target
                    },
                    ChangeBlockType.Decrease => block.Operator switch
                    {
                        ChangeBlockOperator.LessOrEqual => diff < 0 && Math.Abs(diff) <= block.Target,
                        ChangeBlockOperator.GreaterOrEqual => diff < 0 && Math.Abs(diff) >= block.Target
                    },
                    ChangeBlockType.Cross => diff >= 0 ? newResult >= block.Target : newResult <= block.Target
                };

                await context.RespondAsync(new EvaluateBlock.Response { Result = result });
            }
        }
    }
}
