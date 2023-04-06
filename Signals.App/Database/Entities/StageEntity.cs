using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class StageEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SignalId { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
    }
}
