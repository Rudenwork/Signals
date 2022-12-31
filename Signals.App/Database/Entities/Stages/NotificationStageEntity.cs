using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities.Stages
{
    public class NotificationStageEntity : StageEntity
    {
        public Guid ChannelId { get; set; }

        [MaxLength(100)]
        public string Text { get; set; }
    }
}
