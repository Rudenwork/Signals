using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class StageExecutionEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid StageId { get; set; }
        public Guid SignalId { get; set; }
        public int RetryAttempt { get; set; }
        public DateTime ScheduledOn { get; set; }
    }
}
