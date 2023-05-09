using FluentValidation;
using System.Text.Json.Serialization;

namespace Signals.App.Controllers.Models
{
    [JsonDerivedType(typeof(BollingerBands), nameof(BollingerBands))]
    [JsonDerivedType(typeof(Candle), nameof(Candle))]
    [JsonDerivedType(typeof(Constant), nameof(Constant))]
    [JsonDerivedType(typeof(ExponentialMovingAverage), nameof(ExponentialMovingAverage))]
    [JsonDerivedType(typeof(RelativeStrengthIndex), nameof(RelativeStrengthIndex))]
    [JsonDerivedType(typeof(SimpleMovingAverage), nameof(SimpleMovingAverage))]
    public class IndicatorModel
    {
        public IntervalEnum? Interval { get; set; }
        public int? LoopbackPeriod { get; set; }
        public string? Symbol { get; set; }

        public enum IntervalEnum
        {
            OneSecond,
            OneMinute,
            ThreeMinutes,
            FiveMinutes,
            FifteenMinutes,
            ThirtyMinutes,
            OneHour,
            TwoHour,
            FourHour,
            SixHour,
            EightHour,
            TwelveHour,
            OneDay,
            ThreeDay,
            OneWeek,
            OneMonth
        }

        public class Validator : AbstractValidator<IndicatorModel>
        {
            public Validator(BollingerBands.Validator bollingerBands, Candle.Validator candle, Constant.Validator constant)
            {
                RuleFor(x => x)
                    .SetInheritanceValidator(x => 
                    {
                        x.Add(bollingerBands);
                        x.Add(candle);
                        x.Add(constant);
                    });

                When(x => x is Constant, () =>
                {
                    RuleFor(x => x.Symbol)
                        .Null();

                    RuleFor(x => x.Interval)
                        .Null();

                    RuleFor(x => x.LoopbackPeriod)
                        .Null();
                })
                .Otherwise(() =>
                {
                    RuleFor(x => x.Symbol)
                        .NotEmpty();

                    RuleFor(x => x.Interval)
                        .NotNull();

                    When(x => x is Candle, () =>
                    {
                        RuleFor(x => x.LoopbackPeriod)
                            .Null();
                    })
                    .Otherwise(() =>
                    {
                        RuleFor(x => x.LoopbackPeriod)
                            .NotNull()
                            .InclusiveBetween(Constants.LoopBackPeriod.Min, Constants.LoopBackPeriod.Max);
                    });
                });
            }
        }

        public class BollingerBands : IndicatorModel
        {
            public TypeEnum? BandType { get; set; }
            public enum TypeEnum
            {
                Lower,
                Middle,
                Upper
            }

            public new class Validator : AbstractValidator<BollingerBands>
            {
                public Validator()
                {
                    RuleFor(x => x.BandType)
                        .NotNull();
                }
            }
        }

        public class Candle : IndicatorModel 
        {
            public ParameterEnum? ParameterType { get; set; }

            public enum ParameterEnum
            {
                Open,
                Close,
                Low,
                High,
                Average,
                Volume
            }

            public new class Validator : AbstractValidator<Candle>
            {
                public Validator()
                {
                    RuleFor(x => x.ParameterType)
                        .NotNull();
                }
            }
        }

        public class Constant : IndicatorModel
        {
            public decimal? Value { get; set; }

            public new class Validator : AbstractValidator<Constant>
            {
                public Validator()
                {
                    RuleFor(x => x.Value)
                        .NotNull();
                }
            }
        }

        public class ExponentialMovingAverage : IndicatorModel 
        {

        }

        public class RelativeStrengthIndex : IndicatorModel 
        {

        }

        public class SimpleMovingAverage : IndicatorModel 
        {

        }

        private static class Constants 
        {
            public static class LoopBackPeriod 
            {
                public const int Min = 1;
                public const int Max = 100;
            }
        }
    }
}
