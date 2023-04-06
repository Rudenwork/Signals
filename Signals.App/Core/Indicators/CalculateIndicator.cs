using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;
using Mapster;
using MassTransit;
using MassTransit.Mediator;
using Signals.App.Database.Entities;
using Signals.App.Database.Entities.Indicators;
using Skender.Stock.Indicators;

namespace Signals.App.Core.Indicators
{
    public class CalculateIndicator
    {
        public class Request : Request<Response>
        {
            public IndicatorEntity Indicator { get; set; }
            public DateTime? Time { get; set; }
        }

        public class Response
        {
            public decimal Result { get; set; }
        }

        public class Consumer : IConsumer<Request>
        {
            private IMediator Mediator { get; }
            private IBinanceClient BinanceClient { get; }

            public Consumer(IMediator mediator, IBinanceClient binanceClient)
            {
                Mediator = mediator;
                BinanceClient = binanceClient;
            }

            public async Task Consume(ConsumeContext<Request> context)
            {
                if (context.Message.Indicator is ConstantIndicatorEntity constantIndicator)
                {
                    await context.RespondAsync(new Response { Result = constantIndicator.Value });
                    return;
                }

                var symbol = context.Message.Indicator.Symbol;
                var interval = context.Message.Indicator.Interval!.Value.Adapt<KlineInterval>();
                var endTime = context.Message.Time;
                var limit = context.Message.Indicator is CandleIndicatorEntity ? 1 : 100;

                var kLinesResult = await BinanceClient.SpotApi.ExchangeData.GetKlinesAsync(symbol!, interval, null, endTime, limit);

                var quotes = kLinesResult.Data
                    .Select(kline => new Quote
                    {
                        Date = kline.OpenTime,
                        Open = kline.OpenPrice,
                        High = kline.HighPrice,
                        Low = kline.LowPrice,
                        Close = kline.ClosePrice,
                        Volume = kline.Volume,
                    })
                    .ToList();

                var response = context.Message.Indicator switch
                {
                    CandleIndicatorEntity indicator => await Mediator.SendRequest(new CalculateCandleIndicator.Request
                    {
                        Indicator = indicator,
                        Quote = quotes.Last()
                    }),
                    RelativeStrengthIndexIndicatorEntity indicator => await Mediator.SendRequest(new CalculateRelativeStrengthIndexIndicator.Request
                    {
                        Indicator = indicator,
                        Quotes = quotes
                    }),
                    ExponentialMovingAverageIndicatorEntity indicator => await Mediator.SendRequest(new CalculateExponentialMovingAverageIndicator.Request
                    {
                        Indicator = indicator,
                        Quotes = quotes
                    }),
                    SimpleMovingAverageIndicatorEntity indicator => await Mediator.SendRequest(new CalculateSimpleMovingAverageIndicator.Request
                    {
                        Indicator = indicator,
                        Quotes = quotes
                    }),
                    BollingerBandsIndicatorEntity indicator => await Mediator.SendRequest(new CalculateBollingerBandsIndicator.Request
                    {
                        Indicator = indicator,
                        Quotes = quotes
                    })
                };

                await context.RespondAsync(response);
            }
        }
    }
}
