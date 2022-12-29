using MassTransit.Mediator;
using MassTransit;
using Signals.App.Database.Entities.Indicators;
using Skender.Stock.Indicators;

namespace Signals.App.Core.Indicators
{
    public class CalculateBollingerBandsIndicator
    {
        public class Request : Request<CalculateIndicator.Response>
        {
            public BollingerBandsIndicatorEntity Indicator { get; set; }
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
                Logger.LogInformation($"Calculate Bollinger Bands Indicator");

                var bollingerBandResult = context.Message.Quotes
                    .GetBollingerBands(context.Message.Indicator.LoopbackPeriod)
                    .Last();

                var result = context.Message.Indicator.BandType switch
                {
                    BollingerBand.Lower => bollingerBandResult.LowerBand,
                    BollingerBand.Middle => bollingerBandResult.Sma,
                    BollingerBand.Upper => bollingerBandResult.UpperBand
                };

                await context.RespondAsync(new CalculateIndicator.Response { Result = Convert.ToDecimal(result) });
            }
        }
    }
}
