using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class StageEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SignalId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public Guid? PreviousStageId { get; set; }
        public Guid? NextStageId { get; set; }   
    }
}
