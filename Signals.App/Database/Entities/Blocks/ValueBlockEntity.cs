using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Blocks
{
    public class ValueBlockEntity : BlockEntity
    {
        [Column(TypeName = "nvarchar(max)")]
        public ValueBlockOperator Operator { get; set; }
    }

    public enum ValueBlockOperator
    {
        GreaterOrEqual,
        LessOrEqual
    }
}
