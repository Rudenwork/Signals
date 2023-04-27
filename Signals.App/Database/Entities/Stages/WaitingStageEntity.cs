using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Signals.App.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Stages
{
    public class WaitingStageEntity : StageEntity
    {
        [Column(TypeName = "varchar")]
        public TimeUnit Unit { get; set; }

        public int Length { get; set; }
    }
}
