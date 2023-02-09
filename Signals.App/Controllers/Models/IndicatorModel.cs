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
        public Guid? Id { get; set; }
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

        public class BollingerBands : IndicatorModel
        {
            public TypeEnum? BandType { get; set; }
            public enum TypeEnum
            {
                Lower,
                Middle,
                Upper
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
        }

        public class Constant : IndicatorModel
        {
            public decimal? Value { get; set; }
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
    }
}
