namespace Signals.App.Database.Entities.Channels
{
    public class TelegramChannelEntity : ChannelEntity
    {
        public string Username { get; set; }
        public long? ChatId { get; set; }
    }
}
