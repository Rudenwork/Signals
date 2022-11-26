using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class NotificationStageEntity : StageEntity
    {
        public Guid ChannelId { get; set; }

        [MaxLength(100)]
        public string Message { get; set; }
    }
}
