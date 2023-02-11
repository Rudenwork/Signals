using FluentValidation;

namespace Signals.App.Controllers.Models
{
    public abstract class SignalModel
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Name { get; set; }
        public string? Schedule { get; set; }
        public List<StageModel>? Stages { get; set; }
        public bool? IsDisabled { get; set; }
        public ExecutionModel? Execution { get; set; }

        public class Validator : AbstractValidator<SignalModel> 
        {
            public Validator(StageModel.Validator stage) 
            {
                RuleForEach(x => x.Stages)
                    .SetValidator(stage);

                RuleFor(x => x.Name)
                    .MaximumLength(Constants.Name.MaxLength);

                ///TODO: Is schedule validation 
                RuleFor(x => x.Schedule)
                    .MaximumLength(Constants.Schedule.MaxLength);

                RuleFor(x => x.Id)
                    .Null();

                RuleFor(x => x.UserId)
                    .Null();

                RuleFor(x => x.Execution)
                    .Null();
            }
        }

        public class Create : SignalModel
        {
            public new class Validator : AbstractValidator<Create> 
            {
                public Validator(SignalModel.Validator signal) 
                {
                    Include(signal);

                    RuleFor(x => x.Name)
                        .NotEmpty();

                    RuleFor(x => x.Schedule)
                        .NotEmpty();

                    RuleFor(x => x.Stages)
                        .NotEmpty();
                }
            }
        }

        public class Update : SignalModel
        {
            public new class Validator : AbstractValidator<Update>
            {
                public Validator(SignalModel.Validator signal)
                {
                    Include(signal);

                    RuleFor(x => x.IsDisabled)
                        .Null();
                }
            }
        }

        public class Read : SignalModel 
        {
            public class Filter
            {
                public string? Name { get; set; }
                public StatusEnum? Status { get; set; }

                public enum StatusEnum
                {
                    InProgress,
                    Scheduled,
                    Disabled
                }
            }
        }

        private static class Constants
        {
            public static class Name
            {
                public const int MaxLength = 100;
            }

            public static class Schedule
            {
                public const int MaxLength = 50;
            }
        }
    }
}
