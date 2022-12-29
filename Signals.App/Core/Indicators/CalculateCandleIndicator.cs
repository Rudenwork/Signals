using MassTransit.Mediator;
using MassTransit;
using Signals.App.Database.Entities.Indicators;
using Skender.Stock.Indicators;

namespace Signals.App.Core.Indicators
{
    public class CalculateCandleIndicator
    {
        public class Request : Request<CalculateIndicator.Response>
        {
            public CandleIndicatorEntity Indicator { get; set; }
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
                Logger.LogInformation($"Calculate Candle Indicator");

                var quote = context.Message.Quotes.Last();

                var result = context.Message.Indicator.ParameterType switch
                {
                    CandleParameter.Open => quote.Open,
                    CandleParameter.Close => quote.Close,
                    CandleParameter.Low => quote.Low,
                    CandleParameter.High => quote.High,
                    CandleParameter.Average => (quote.High + quote.Low) / 2, 
                    CandleParameter.Volume => quote.Volume
                };

                await context.RespondAsync(new CalculateIndicator.Response { Result = result });
            }
        }
    }
}

