using FluentValidation;

namespace Signals.App.Controllers.Models
{
    public class ChannelModel
    {
        public class Create
        {
            public Type? Type { get; set; }
            public string? Destination { get; set; }
            public string? Description { get; set; }

            public class Validator : AbstractValidator<Create>
            {
                public Validator()
                {
                    RuleFor(x => x.Type).NotNull();

                    RuleFor(x => x.Destination)
                        .NotNull()
                        .MaximumLength(100);

                    RuleFor(x => x.Destination)
                        .EmailAddress()
                        .When(x => x.Type is ChannelModel.Type.Email);

                    RuleFor(x => x.Description)
                        .MaximumLength(100);
                }
            }
        }

        public class Read 
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public Type Type { get; set; }
            public string Destination { get; set; }
            public string Description { get; set; }
            public bool IsVerified { get; set; }
        }

        public class Update
        {
            public Type? Type { get; set; }
            public string? Destination { get; set; }
            public string? Description { get; set; }

            public class Validator : AbstractValidator<Update>
            {
                public Validator()
                {
                    RuleFor(x => x.Type)
                        .NotNull()
                        .When(x => x.Destination is not null);

                    RuleFor(x => x.Destination)
                        .NotNull()
                        .When(x => x.Type is not null);

                    RuleFor(x => x.Destination)
                        .MaximumLength(100);

                    RuleFor(x => x.Destination)
                        .EmailAddress()
                        .When(x => x.Type is ChannelModel.Type.Email);

                    RuleFor(x => x.Description)
                        .MaximumLength(100);
                }
            }
        }

        public class Filter
        {
            public Type? Type { get; set; }
            public string? Destination { get; set; }
            public string? Description { get; set; }
            public bool? IsVerified { get; set; }
        }

        public enum Type
        {
            Email
        }
    }
}
