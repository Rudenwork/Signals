using FluentValidation;
using System.Text.Json.Serialization;

namespace Signals.App.Controllers.Models
{
    public abstract class ChannelModel
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

        [JsonDerivedType(typeof(Email), nameof(Email))]
        [JsonDerivedType(typeof(Telegram), nameof(Telegram))]
        public abstract class Create : ChannelModel
        {
            public class Email : Create
            {
                public string? Address { get; set; }

                public new class Validator : AbstractValidator<Email>
                {
                    public Validator(ChannelModel.Validator validator)
                    {
                        Include(validator);

                        RuleFor(x => x.Address)
                            .NotNull()
                            .EmailAddress();
                    }
                }
            }

            public class Telegram : Create
            {
                public string? Username { get; set; }

                public new class Validator : AbstractValidator<Telegram>
                {
                    public Validator(ChannelModel.Validator validator)
                    {
                        Include(validator);

                        RuleFor(x => x.Username)
                            .NotNull()
                            .MaximumLength(100)
                            .Matches(Constants.Username.Regex);
                    }
                }
            }
        }

        [JsonDerivedType(typeof(Email), nameof(Email))]
        [JsonDerivedType(typeof(Telegram), nameof(Telegram))]
        public abstract class Update : ChannelModel
        {
            public class Email : Update
            {
                public string? Address { get; set; }

                public new class Validator : AbstractValidator<Email>
                {
                    public Validator(ChannelModel.Validator validator)
                    {
                        Include(validator);

                        RuleFor(x => x.Address)
                            .EmailAddress();
                    }
                }
            }

            public class Telegram : Update
            {
                public string? Username { get; set; }

                public new class Validator : AbstractValidator<Telegram>
                {
                    public Validator(ChannelModel.Validator validator)
                    {
                        Include(validator);

                        RuleFor(x => x.Username)
                            .MaximumLength(100)
                            .Matches(Constants.Username.Regex);
                    }
                }
            }
        }

        [JsonDerivedType(typeof(Email), nameof(Email))]
        [JsonDerivedType(typeof(Telegram), nameof(Telegram))]
        public abstract class Read : ChannelModel
        {
            public class Email : Read
            {
                public string? Address { get; set; }
            }

            public class Telegram : Read
            {
                public string? Username { get; set; }
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
