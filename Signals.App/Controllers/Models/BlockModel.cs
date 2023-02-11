using FluentValidation;
using System.Text.Json.Serialization;

namespace Signals.App.Controllers.Models
{
    [JsonDerivedType(typeof(Group), nameof(Group))]
    [JsonDerivedType(typeof(Value), nameof(Value))]
    [JsonDerivedType(typeof(Change), nameof(Change))]
    public abstract class BlockModel
    {
        public Guid? Id { get; set; }

        public class Validator : AbstractValidator<BlockModel>
        {
            public Validator(Group.Validator group, Value.Validator value, Change.Validator change)
            {
                RuleFor(x => x)
                    .SetInheritanceValidator(x => 
                    {
                        x.Add(group);
                        x.Add(value);
                        x.Add(change);
                    });

                RuleFor(x => x.Id)
                    .Null();
            }
        }

        public class Group : BlockModel
        {
            public TypeEnum? Type { get; set; }
            public List<BlockModel>? Children { get; set; }

            public enum TypeEnum
            {
                And,
                Or
            }

            public new class Validator : AbstractValidator<Group>
            {
                public Validator(/*BlockModel.Validator block*/)
                {
                    RuleFor(x => x.Type)
                        .NotNull();

                    RuleFor(x => x.Children)
                        .NotEmpty();

                    //RuleForEach(x => x.Children)
                    //    .SetValidator(block);
                }
            }
        }

        public class Value : BlockModel
        {
            public IndicatorModel? LeftIndicator { get; set; }
            public IndicatorModel? RightIndicator { get; set; }
            public OperatorEnum? Operator { get; set; }

            public enum OperatorEnum
            {
                GreaterOrEqual,
                LessOrEqual
            }

            public new class Validator : AbstractValidator<Value>
            {
                public Validator(IndicatorModel.Validator indicator)
                {
                    RuleFor(x => x.LeftIndicator)
                        .NotNull()
                        .SetValidator(indicator);

                    RuleFor(x => x.RightIndicator)
                        .NotNull()
                        .SetValidator(indicator);

                    ///TODO: check behaviour of IsInEnum()
                    RuleFor(x => x.Operator)
                        .NotNull();
                }
            }
        }
        
        public class Change : BlockModel
        {
            public IndicatorModel? Indicator { get; set; }
            public TypeEnum? Type { get; set; }
            public OperatorEnum? Operator { get; set; }
            public decimal? Target { get; set; }
            public bool? IsPercentage { get; set; }
            public TimeSpan? Period { get; set; }

            public enum TypeEnum
            {
                Increase,
                Decrease,
                Cross
            }

            public enum OperatorEnum
            {
                GreaterOrEqual,
                LessOrEqual,
                Crossed
            }

            public new class Validator : AbstractValidator<Change>
            {
                public Validator(IndicatorModel.Validator indicator)
                {
                    RuleFor(x => x.Indicator)
                        .NotNull()
                        .SetValidator(indicator);

                    RuleFor(x => x.Type)
                        .NotNull();

                    RuleFor(x => x.Operator)
                        .NotNull();

                    RuleFor(x => x.Target)
                        .NotNull();

                    RuleFor(x => x.IsPercentage)
                        .NotNull();

                    RuleFor(x => x.Period)
                        .NotNull();
                }
            }
        }
    }
}
