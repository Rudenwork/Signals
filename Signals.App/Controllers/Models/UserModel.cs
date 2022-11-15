using System.ComponentModel.DataAnnotations;

namespace Signals.App.Controllers.Models
{
    public class UserModel
    {
        public class Create
        {
            [Required]
            [RegularExpression(Constants.Username.Regex, ErrorMessage = Constants.Username.ErrorMessage)]
            public string Username { get; set; }

            [Required]
            [RegularExpression(Constants.Password.Regex, ErrorMessage = Constants.Password.ErrorMessage)]
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
            [RegularExpression(Constants.Username.Regex, ErrorMessage = Constants.Username.ErrorMessage)]
            public string? Username { get; set; }

            [RegularExpression(Constants.Password.Regex, ErrorMessage = Constants.Password.ErrorMessage)]
            public string? Password { get; set; }

            public bool? IsAdmin { get; set; }
            public bool? IsDisabled { get; set; }
        }

        private static class Constants
        {
            public static class Username
            {
                public const string Regex = @"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{1,50}$";
                public const string ErrorMessage = "May only contain Roman letters (a-z, A-Z), digits (0-9), and some special characters (._). Max length 50 symbols. Cannot begin with a period.";
            }

            public static class Password
            {
                public const string Regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
                public const string ErrorMessage = "Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character";
            }
        }
    }
}
