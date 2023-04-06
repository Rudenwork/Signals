using MassTransit.Mediator;
using MassTransit;
using Signals.App.Database.Entities.Indicators;
using Skender.Stock.Indicators;

namespace Signals.App.Core.Indicators
{
    public class CalculateExponentialMovingAverageIndicator
    {
        public class Request : Request<CalculateIndicator.Response>
        {
            public ExponentialMovingAverageIndicatorEntity Indicator { get; set; }
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
                Logger.LogInformation($"Calculate Exponential Moving Average Indicator");

                var result = context.Message.Quotes
                    .GetEma(context.Message.Indicator.LoopbackPeriod!.Value)
                    .Last();

                await context.RespondAsync(new CalculateIndicator.Response { Result = Convert.ToDecimal(result.Ema) });
            }
        }
    }
}
