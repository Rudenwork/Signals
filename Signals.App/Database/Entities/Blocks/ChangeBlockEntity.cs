namespace Signals.App.Database.Entities.Blocks
{
    public class ChangeBlockEntity : BlockEntity
    {
        public Guid IndicatorId { get; set; }
        public ChangeBlockType Type { get; set; }
        public ChangeBlockOperator Operator { get; set; }
        public decimal Target { get; set; }
        public bool IsPercentage { get; set; }
        public TimeSpan Period { get; set; }
    }

    public enum ChangeBlockType
    {
        Increase,
        Decrease,
        Cross
    }

    public enum ChangeBlockOperator
    {
        GreaterOrEqual,
        LessOrEqual,
        Crossed
    }
}
