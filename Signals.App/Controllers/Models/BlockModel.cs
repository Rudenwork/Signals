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

        public class Group : BlockModel
        {
            public TypeEnum? Type { get; set; }
            public List<BlockModel>? Children { get; set; }

            public enum TypeEnum
            {
                And,
                Or
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
        }
    }

    [JsonDerivedType(typeof(Group), nameof(Group))]
    [JsonDerivedType(typeof(Value), nameof(Value))]
    [JsonDerivedType(typeof(Change), nameof(Change))]
    public abstract class BlockModel_X
    {
        public Guid? Id { get; set; }
        public class Validator : AbstractValidator<BlockModel_X>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .Null();
            }
        }

        public class Group : BlockModel_X 
        {
            public TypeEnum Type { get; set; }

            public List<BlockModel_X>? Children { get; set; }

            public new class Validator : AbstractValidator<Group>
            {
                public Validator(BlockModel_X.Validator validator)
                {
                    Include(validator);
                }
            }

            public enum TypeEnum
            {
                And,
                Or
            }
        }
        public class Value : BlockModel_X 
        {
            /// TODO: Create model for indicator
            public Guid? LeftIndicatorId { get; set; }
            public Guid? RightIndicatorId { get; set; }

            public new class Validator : AbstractValidator<Value>
            {
                public Validator(BlockModel_X.Validator validator)
                {
                    Include(validator);
                }
            }

            public OperatorEnum Operator { get; set; }

            public enum OperatorEnum
            {
                GreaterOrEqual,
                LessOrEqual
            }
        }
        public class Change : BlockModel_X 
        {
            /// TODO: Create model for indicator
            public Guid? IndicatorId { get; set; }

            public TypeEnum Type { get; set; }
            public OperatorEnum Operator { get; set; }
            public decimal? Target { get; set; }
            public bool? IsPercentage { get; set; }
            public TimeSpan? Period { get; set; }

            public new class Validator : AbstractValidator<Change>
            {
                public Validator(BlockModel_X.Validator validator)
                {
                    Include(validator);
                }
            }

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
        }
    }
}
