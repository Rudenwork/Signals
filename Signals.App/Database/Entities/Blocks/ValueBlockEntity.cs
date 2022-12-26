namespace Signals.App.Database.Entities.Blocks
{
    public class ValueBlockEntity : BlockEntity
    {
        public Guid LeftIndicatorId { get; set; }
        public Guid RightIndicatorId { get; set;}
        public ValueBlockOperator Operator { get; set; }
    }

    public enum ValueBlockOperator
    {
        GreaterOrEqual,
        LessOrEqual
    }
}
