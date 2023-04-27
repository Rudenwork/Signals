using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Signals.App.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Stages
{
    public class ConditionStageEntity : StageEntity
    {
        public int? RetryCount { get; set; }

        [Column(TypeName = "varchar")]
        public TimeUnit? RetryDelayUnit { get; set; }

        public int? RetryDelayLength { get; set; }
        public Guid BlockId { get; set; }

        //EF Injected
        public BlockEntity Block { get; set; }
    }
}
