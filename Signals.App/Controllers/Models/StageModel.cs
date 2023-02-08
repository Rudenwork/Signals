using FluentValidation;
using System.Text.Json.Serialization;

namespace Signals.App.Controllers.Models
{
    [JsonDerivedType(typeof(Condition), nameof(Condition))]
    [JsonDerivedType(typeof(Waiting), nameof(Waiting))]
    [JsonDerivedType(typeof(Notification), nameof(Notification))]
    public abstract class StageModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }

        public class Condition : StageModel
        {
            public int? RetryCount { get; set; }
            public TimeSpan? RetryDelay { get; set; }
            public BlockModel? Block { get; set; }
        }

        public class Waiting : StageModel
        {
            public TimeSpan? Period { get; set; }
        }

        public class Notification : StageModel
        {
            public Guid? ChannelId { get; set; }
            public string? Text { get; set; }
        }
    }

    [JsonDerivedType(typeof(Condition), nameof(Condition))]
    [JsonDerivedType(typeof(Waiting), nameof(Waiting))]
    [JsonDerivedType(typeof(Notification), nameof(Notification))]
    public abstract class StageModel_X 
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }

        public class Validator : AbstractValidator<StageModel_X> 
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .MaximumLength(Constants.Name.MaxLength);

                RuleFor(x => x.Id)
                    .Null();
            }
        }

        public class Condition : StageModel_X
        {
            public int? RetryCount { get; set; }
            public TimeSpan? RetryDelay { get; set; }
            public BlockModel_X? Block { get; set; }

            public new class Validator : AbstractValidator<Condition>
            {
                public Validator(StageModel_X.Validator validator)
                {
                    Include(validator);

                    RuleFor(x => x.RetryCount)
                        .LessThanOrEqualTo(Constants.RetryCount.MaxCount)
                        .NotNull();

                    RuleFor(x => x.RetryDelay)
                        .NotNull();
                }
            }

            public class Create : Condition
            {
                public new class Validator : AbstractValidator<Create>
                {
                    public Validator(Condition.Validator validator)
                    {
                        Include(validator);

                        RuleFor(x => x.Block)
                            .NotNull();
                    }
                }
            }

            public class Update : Condition
            {
                public new class Validator : AbstractValidator<Update>
                {
                    public Validator(Condition.Validator validator)
                    {
                        Include(validator);
                    }
                }
            }

            public class Read : Condition {}
        }

        public class Waiting : StageModel_X 
        {
            public TimeSpan? Period { get; set; }

            public new class Validator : AbstractValidator<Waiting>
            {
                public Validator(StageModel_X.Validator validator)
                {
                    Include(validator);
                }
            }

            public class Create : Waiting 
            {
                public new class Validator : AbstractValidator<Create>
                {
                    public Validator(Waiting.Validator validator)
                    {
                        Include(validator);

                        RuleFor(x => x.Period)
                            .NotNull();
                    }
                }
            }

            public class Update : Waiting
            {
                public new class Validator : AbstractValidator<Update>
                {
                    public Validator(Waiting.Validator validator)
                    {
                        Include(validator);
                    }
                }
            }

            public class Read : Waiting {}
        }
        public class Notification : StageModel_X
        {
            ///TODO:ChannelId --> ChannelModel property?
            public Guid? ChannelId { get; set; }
            public string? Text { get; set; }

            public new class Validator : AbstractValidator<Notification>
            {
                public Validator(StageModel_X.Validator validator)
                {
                    Include(validator);
                }
            }

            public class Create : Notification 
            {
                public new class Validator : AbstractValidator<Create>
                {
                    public Validator(Notification.Validator validator)
                    {
                        Include(validator);

                        RuleFor(x => x.ChannelId)
                            .NotNull();

                        RuleFor(x => x.Text)
                            .NotEmpty()
                            .MaximumLength(Constants.Text.MaxLength);
                    }
                }
            }
            public class Update : Notification 
            {
                public new class Validator : AbstractValidator<Create>
                {
                    public Validator(Notification.Validator validator)
                    {
                        Include(validator);

                        RuleFor(x => x.Text)
                            .MinimumLength(Constants.Text.MinLength)
                            .MaximumLength(Constants.Text.MaxLength);
                    }
                }
            }
            public class Read : Notification { }
        }

        public class Filter
        {
            public string? Name { get; set; }
        }

        private static class Constants
        {
            public static class Name
            {
                ///TODO: specify value
                public const int MaxLength = 100;
            }

            public static class RetryCount
            {
                ///TODO: specify value
                public const int MaxCount = 100;
            }

            public static class Text
            {
                public const int MinLength = 1;
                ///TODO: specify value
                public const int MaxLength = 1000;
            }
        }
    }
}
