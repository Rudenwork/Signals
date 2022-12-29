using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Blocks
{
    public class ChangeBlockEntity : BlockEntity
    {
        public Guid IndicatorId { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public ChangeBlockType Type { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public ChangeBlockOperator Operator { get; set; }
        public decimal Target { get; set; }
        public bool IsPercentage { get; set; }
        public TimeSpan Period { get; set; }

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
