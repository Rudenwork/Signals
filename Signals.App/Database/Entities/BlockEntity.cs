using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities
{
    public class BlockEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid StageId { get; set; }
        public Guid? ParentBlockId { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public BlockType Type { get; set; }

        //Related Entities
        public List<BlockParameterEntity> Parameters { get; set; }

        public enum BlockType
        {
            Group,
            Change,
            Value
        }
    }
}
