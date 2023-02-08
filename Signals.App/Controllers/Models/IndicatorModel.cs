using System.Text.Json.Serialization;

namespace Signals.App.Controllers.Models
{
    [JsonDerivedType(typeof(Constant), nameof(Constant))]
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
        
        public class Constant : IndicatorModel
        {
            public decimal? Value { get; set; }
        }
    }
}
