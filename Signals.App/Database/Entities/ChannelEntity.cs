namespace Signals.App.Database.Entities
{
    public class ChannelEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ChannelType Type { get; set; }
        public string Destination { get; set; }
        public bool IsVerified { get; set; }
        public string? Description { get; set; }

        public enum ChannelType 
        {
            Email
        }
    }
}
