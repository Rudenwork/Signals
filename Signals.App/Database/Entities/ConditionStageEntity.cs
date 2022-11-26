using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities
{
    public class ConditionStageEntity : StageEntity
    {
        [Column(TypeName = "nvarchar(10)")]
        public ConditionType Type { get; set; }

        public int? RetryCount { get; set; }
        public TimeSpan? RetryDelay { get; set; }

        public enum ConditionType
        {
            And,
            Or
        }
    }
}
