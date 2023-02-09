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

        public class Create : SignalModel
        {
            
        }
    }

    public abstract class SignalModel_X
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Name { get; set; }
        public string? Schedule { get; set; }
        public bool? IsDisabled { get; set; }

        ///TODO: Raw data: Execution
        public bool? HasExecution { get; set; }

        ///TODO: CurrentStage property
        public StageModel_X? CurrentStage { get; set; }
        ///TODO: Stages property validate
        public List<StageModel_X>? Stages { get; set; }

        ///TODO: Status property

        ///TODO: Mirro pages properties 

        public class Validator : AbstractValidator<SignalModel_X>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .MaximumLength(Constants.Name.MaxLength);

                RuleFor(x => x.Schedule)
                    .MaximumLength(Constants.Schedule.MaxLength);

                RuleFor(x => x.Id)
                    .Null();

                RuleFor(x => x.UserId)
                    .Null();          
            }
        }

        public class Create : SignalModel_X 
        {
            public new class Validator : AbstractValidator<Create>
            {
                public Validator(SignalModel_X.Validator validator)
                {
                    Include(validator);

                    RuleFor(x => x.Name)
                        .NotEmpty();
                    
                    RuleFor(x => x.Schedule)
                        .NotEmpty();

                    RuleFor(x => x.Stages)
                        .NotEmpty();
                }
            }
        }

        public class Update : SignalModel_X
        {
            public new class Validator : AbstractValidator<Update>
            {
                public Validator(SignalModel_X.Validator validator)
                {
                    Include(validator);

                    RuleFor(x => x.IsDisabled)
                        .Null();
                }
            }
        }

        public class Read : SignalModel_X 
        {
            public class Filter 
            {
                public string? Name { get; set; }
                public string? Schedule { get; set; }
                public StatusEnum Status { get; set; }

                ///TODO: remove IsDisabled
                public bool? IsDisabled { get; set; }

                ///TODO: Check enum name
                public enum StatusEnum
                {
                    InProgress,
                    Scheduled,
                    Disabled
                }
            }
        }

        ///TODO: specify validations for schedule
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
