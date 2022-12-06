using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities
{
    public class StageEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SignalId { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public StageType Type { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public Guid? PreviousStageId { get; set; }
        public Guid? NextStageId { get; set; }

        //Related Entities
        public List<StageParameterEntity> Parameters { get; set; }

        public enum StageType
        {
            Condition,
            Waiting,
            Notification
        }
    }
}
