using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities
{
    public class IndicatorEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "varchar")]
        public Interval Interval { get; set; }

        public int LoopbackPeriod { get; set; }
        public string? Symbol { get; set; }
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
