using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities
{
    public class BlockEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? ParentBlockId { get; set; }
    }
}
