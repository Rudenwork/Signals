using MassTransit;
using MassTransit.Mediator;
using Signals.App.Database.Entities.Indicators;
using Skender.Stock.Indicators;

namespace Signals.App.Core.Indicators
{
    public class CalculateRelativeStrengthIndexIndicator
    {
        public class Request : Request<CalculateIndicator.Response>
        {
            public RelativeStrengthIndexIndicatorEntity Indicator { get; set; }
            public List<Quote> Quotes { get; set; }
        }

        public class Consumer : IConsumer<Request>
        {
            private ILogger<Consumer> Logger { get; }

            public Consumer(ILogger<Consumer> logger)
            {
                Logger = logger;
            }

            public async Task Consume(ConsumeContext<Request> context)
            {
                Logger.LogInformation($"Calculate Relative Strength Index Indicator");

                var result = context.Message.Quotes
                    .GetRsi(context.Message.Indicator.LoopbackPeriod)
                    .Last();

                await context.RespondAsync(new CalculateIndicator.Response { Result = Convert.ToDecimal(result.Rsi) });
            }
        }
    }
}
