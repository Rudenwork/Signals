using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class SignalEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Schedule { get; set; }
        public bool IsDisabled { get; set; }

        //EF Injected
        public List<StageEntity> Stages { get; set; }
    }
}
