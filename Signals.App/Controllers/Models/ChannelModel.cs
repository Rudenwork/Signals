using FluentValidation;

namespace Signals.App.Controllers.Models
{
    public abstract class ChannelModel
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public TypeEnum? Type { get; set; }
        public string? Destination { get; set; }
        public string? Description { get; set; }
        public bool? IsVerified { get; set; }

        public class Validator : AbstractValidator<ChannelModel>
        {
            public Validator()
            {
                RuleFor(x => x.Type)
                    .IsInEnum();

                When(x => x.Type is TypeEnum.Email, () =>
                {
                    RuleFor(x => x.Destination)
                        .EmailAddress();
                })
                .Otherwise(() =>
                {
                    RuleFor(x => x.Destination)
                        .MaximumLength(Constants.Telegram.Destination.Length)
                        .Matches(Constants.Telegram.Destination.Regex);
                });

                RuleFor(x => x.Description)
                    .MaximumLength(Constants.Description.Length);

                RuleFor(x => x.Id)
                    .Null();

                RuleFor(x => x.UserId)
                    .Null();

                RuleFor(x => x.IsVerified)
                    .Null();
            }
        }

        public class Create : ChannelModel
        {
            public new class Validator : AbstractValidator<Create>
            {
                public Validator(ChannelModel.Validator validator)
                {
                    Include(validator);

                    RuleFor(x => x.Type)
                        .NotNull();

                    RuleFor(x => x.Destination)
                        .NotNull();
                }
            }
        }

        public class Update : ChannelModel
        {
            public class Validator : AbstractValidator<Update>
            {
                public Validator(ChannelModel.Validator validator)
                {
                    Include(validator);

                    When(x => x.Type is not null, () =>
                    {
                        RuleFor(x => x.Destination)
                            .NotNull();
                    });
                }
            }
        }

        public class Read : ChannelModel
        {
            public class Filter
            {
                public TypeEnum? Type { get; set; }
                public string? Description { get; set; }
                public bool? IsVerified { get; set; }
                public string? Destination { get; set; }
            }
        }

        public class Verify
        {
            public string Code { get; set; }
        }

        private static class Constants
        {
            public static class Description
            {
                public const int Length = 100;
            }

            public static class Telegram
            {
                public static class Destination
                {
                    public const int Length = 100;
                    public const string Regex = @"^[a-zA-Z0-9]+([_ -]?[a-zA-Z0-9])*$";
                    public const string ErrorMessage = "Not valid";
                }
            }
        }

        public enum TypeEnum
        {
            Telegram,
            Email
        }
    }
}
