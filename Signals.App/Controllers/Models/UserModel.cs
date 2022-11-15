using System.ComponentModel.DataAnnotations;

namespace Signals.App.Controllers.Models
{
    public class UserModel
    {
        public class Create
        {
            [Required]
            [RegularExpression(@"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{1,50}$",
                ErrorMessage = $"May only contain Roman letters (a-z, A-Z), digits (0-9), and some special characters (._). Max length 50 symbols. Cannot begin with a period.")]
            public string Username { get; set; }
            [Required]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
                ErrorMessage = $"Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
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
            [RegularExpression(@"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{1,50}$",
                ErrorMessage = $"May only contain Roman letters (a-z, A-Z), digits (0-9), and some special characters (._). Max length 50 symbols. Cannot begin with a period.")]
            public string? Username { get; set; }
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
                 ErrorMessage = $"Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
            public string? Password { get; set; }
            public bool? IsAdmin { get; set; }
            public bool? IsDisabled { get; set; }
        }
    }
}
