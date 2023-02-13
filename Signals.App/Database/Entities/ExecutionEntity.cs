using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class ExecutionEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SignalId { get; set; }
        public Guid? StageId { get; set; }

        //EF Injected
        public StageEntity? Stage { get; set; }
    }
}
