using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class SignalExecutionEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SignalId { get; set; }
        public Guid StageId { get; set; }
        public DateTime StageScheduledOn { get; set; }
        public int StageRetryAttempt { get; set; }
    }
}
