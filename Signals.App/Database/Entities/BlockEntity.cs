using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class BlockEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? ParentBlockId { get; set; }
        public int? Index { get; set; }
    }
}
