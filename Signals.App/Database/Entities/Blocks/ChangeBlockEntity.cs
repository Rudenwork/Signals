using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Signals.App.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Blocks
{
    public class ChangeBlockEntity : BlockEntity
    {
        public Guid IndicatorId { get; set; }

        [Column(TypeName = "varchar")]
        public ChangeBlockType Type { get; set; }

        [Column(TypeName = "varchar")]
        public ChangeBlockOperator Operator { get; set; }

        public decimal Target { get; set; }
        public bool IsPercentage { get; set; }

        [Column(TypeName = "varchar")]
        public TimeUnit PeriodUnit { get; set; }

        public int PeriodLength { get; set; }

        //EF Injected
        public IndicatorEntity Indicator { get; set; }
    }

    public enum ChangeBlockType
    {
        Increase,
        Decrease,
        Cross
    }

    public enum ChangeBlockOperator
    {
        GreaterOrEqual,
        LessOrEqual,
        Crossed
    }
}
