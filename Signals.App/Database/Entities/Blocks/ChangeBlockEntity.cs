using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Blocks
{
    public class ChangeBlockEntity : BlockEntity
    {
        [Column(TypeName = "nvarchar(max)")]
        public ChangeBlockType Type { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public ChangeBlockOperator Operator { get; set; }

        public decimal Target { get; set; }
        public bool IsPercentage { get; set; }
        public TimeSpan Period { get; set; }
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
