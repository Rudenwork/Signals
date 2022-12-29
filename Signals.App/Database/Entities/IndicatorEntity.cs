using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities
{
    public class IndicatorEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MinLength(1), MaxLength(100)]
        public int LoopbackPeriod { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public Interval Interval { get; set; }

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
