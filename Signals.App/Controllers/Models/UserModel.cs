using FluentValidation;

namespace Signals.App.Controllers.Models
{
    public abstract class UserModel
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsDisabled { get; set; }

        public class Validator : AbstractValidator<UserModel>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .Null();

                RuleFor(x => x.Username)
                    .Matches(Constants.Username.Regex)
                    .WithMessage(Constants.Username.ErrorMessage);

                RuleFor(x => x.Password)
                    .Matches(Constants.Password.Regex)
                    .WithMessage(Constants.Password.ErrorMessage);
            }
        }

        public class Create : UserModel
        {
            public new class Validator : AbstractValidator<Create>
            {
                public Validator(UserModel.Validator validator)
                {
                    Include(validator);

                    RuleFor(x => x.Username)
                        .NotNull();

                    RuleFor(x => x.Password)
                        .NotNull();
                }
            }
        }

        public class Update : UserModel
        {
            public new class Validator : AbstractValidator<Update>
            {
                public Validator(UserModel.Validator validator)
                {
                    Include(validator);

                    RuleFor(x => x.IsDisabled)
                        .Null();
                }
            }
        }

        public class Read : UserModel
        {
            public class Filter
            {
                public string? Username { get; set; }
                public bool? IsAdmin { get; set; }
                public bool? IsDisabled { get; set; }
            }
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
