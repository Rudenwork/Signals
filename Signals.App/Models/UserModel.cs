using System.ComponentModel.DataAnnotations;

namespace Signals.App.Models
{
    public class UserModel
    {
        public class Create
        {
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }
            public bool? IsAdmin { get; set; }
            public bool? IsDisabled { get; set; }
        }

        public class Read
        {
            public Guid Id { get; set; }
            public string Username { get; set; }
            public bool IsAdmin { get; set; }
            public bool IsDisabled { get; set; }
        }

        public class Update
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
            public bool? IsAdmin { get; set; }
            public bool? IsDisabled { get; set; }
        }
    }
}
