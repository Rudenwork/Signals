namespace Signals.App.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDisabled { get; set; }
    }
}
