using FluentValidation;
using System.Text.Json.Serialization;

namespace Signals.App.Controllers.Models
{
    [JsonDerivedType(typeof(Email), nameof(Email))]
    [JsonDerivedType(typeof(Telegram), nameof(Telegram))]
    public class ChannelModel
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Description { get; set; }
        public bool? IsVerified { get; set; }

        public class Validator : AbstractValidator<ChannelModel>
        {
            public Validator()
            {
                RuleFor(x => x.Description)
                    .MaximumLength(100);

                RuleFor(x => x.Id)
                    .Null();

                RuleFor(x => x.UserId)
                    .Null();

                RuleFor(x => x.IsVerified)
                    .Null();
            }
        }

        public class Email : ChannelModel
        {
            public string? Address { get; set; }

            public class Validator : AbstractValidator<Email>
            {
                public Validator(ChannelModel.Validator baseValidator)
                {
                    Include(baseValidator);

                    RuleFor(x => x.Address)
                        .NotNull()
                        .EmailAddress();
                }
            }
        }

        public class Telegram : ChannelModel
        {
            public string? Username { get; set; }

            public class Validator : AbstractValidator<Telegram>
            {
                public Validator(ChannelModel.Validator baseValidator)
                {
                    Include(baseValidator);

                    RuleFor(x => x.Username)
                        .NotNull()
                        .MaximumLength(100)
                        .Matches(Constants.Username.Regex);
                }
            }
        }

        public class Filter
        {
            public TypeEnum? Type { get; set; }
            public string? Description { get; set; }
            public bool? IsVerified { get; set; }

            //Email
            public string? Address { get; set; }

            //Telegram
            public string? Username { get; set; }

            public enum TypeEnum
            {
                Email,
                Telegram
            }
        }

        private static class Constants
        {
            public static class Username
            {
                public const string Regex = @"^[a-zA-Z0-9]+([_ -]?[a-zA-Z0-9])*$";
                public const string ErrorMessage = "Not valid";
            }
        }
    }
}
