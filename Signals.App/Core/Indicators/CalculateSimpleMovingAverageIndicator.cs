using MassTransit.Mediator;
using MassTransit;
using Signals.App.Database.Entities.Indicators;
using Skender.Stock.Indicators;

namespace Signals.App.Core.Indicators
{
    public class CalculateSimpleMovingAverageIndicator
    {
        public class Request : Request<CalculateIndicator.Response>
        {
            public SimpleMovingAverageIndicatorEntity Indicator { get; set; }
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
                Logger.LogInformation($"Calculate Moving Average Indicator");

                var result = context.Message.Quotes
                    .GetSma(context.Message.Indicator.LoopbackPeriod)
                    .Last();

                await context.RespondAsync(new CalculateIndicator.Response { Result = Convert.ToDecimal(result.Sma) });
            }
        }
    }
}

