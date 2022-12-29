using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Blocks
{
    public class ValueBlockEntity : BlockEntity
    {
        public Guid LeftIndicatorId { get; set; }
        public Guid RightIndicatorId { get; set;}

        [Column(TypeName = "nvarchar(25)")]
        public ValueBlockOperator Operator { get; set; }

        //EF Injected
        public IndicatorEntity LeftIndicator { get; set; }
        public IndicatorEntity RightIndicator { get; set; }
    }

    public enum ValueBlockOperator
    {
        GreaterOrEqual,
        LessOrEqual
    }
}
