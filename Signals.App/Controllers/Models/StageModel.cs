﻿using FluentValidation;
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

        public class Validator : AbstractValidator<StageModel> 
        {
            public Validator(Condition.Validator condition, Waiting.Validator waiting, Notification.Validator notification)
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(Constants.Name.MaxLength);

                RuleFor(x => x.Id)
                    .Null();

                RuleFor(x => x)
                    .SetInheritanceValidator(x => 
                    {
                        x.Add(condition);
                        x.Add(waiting);
                        x.Add(notification);
                    });
            }
        }

        public class Condition : StageModel
        {
            public int? RetryCount { get; set; }
            public TimeSpan? RetryDelay { get; set; }
            public BlockModel? Block { get; set; }

            public new class Validator : AbstractValidator<Condition>
            {
                public Validator(BlockModel.Validator block)
                {
                    RuleFor(x => x.Block)
                        .NotNull()
                        .SetValidator(block);

                    RuleFor(x => x.RetryCount)
                        .NotNull()
                        .InclusiveBetween(Constants.Condition.RetryCount.Min, Constants.Condition.RetryCount.Max);

                    RuleFor(x => x.RetryDelay)
                        .NotNull();
                }
            }
        }

        public class Waiting : StageModel
        {
            public TimeSpan? Period { get; set; }

            public new class Validator : AbstractValidator<Waiting>
            {
                public Validator()
                {
                    RuleFor(x => x.Period)
                        .NotNull();
                }
            }
        }

        public class Notification : StageModel
        {
            public Guid? ChannelId { get; set; }
            public string? Text { get; set; }

            public new class Validator : AbstractValidator<Notification>
            {
                public Validator()
                {
                    RuleFor(x => x.ChannelId)
                        .NotNull();

                    RuleFor(x => x.Text)
                        .NotEmpty()
                        .Length(Constants.Notification.Text.MinLength, Constants.Notification.Text.MaxLength);
                }
            }
        }

        private static class Constants
        {
            public static class Name
            {
                public const int MaxLength = 100;
            }

            public static class Condition
            {
                public static class RetryCount
                {
                    public const int Max = 100;
                    public const int Min = 0;
                }
            }

            public static class Notification
            {
                public static class Text
                {
                    public const int MinLength = 1;

                    public const int MaxLength = 1000;
                }
            }
        }
    }
}