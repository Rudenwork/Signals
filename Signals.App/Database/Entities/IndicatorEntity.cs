using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class IndicatorEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MinLength(1), MaxLength(100)]
        public int LoopbackPeriod { get; set; }

        public Interval Interval { get; set; }

        public string Symbol { get; set; }
    }

    public enum Interval
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
}
