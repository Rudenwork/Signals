using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDisabled { get; set; }
    }
}
