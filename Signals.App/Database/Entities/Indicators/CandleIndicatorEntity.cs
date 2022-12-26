namespace Signals.App.Database.Entities.Indicators
{
    public class CandleIndicatorEntity : IndicatorEntity
    {
        public CandleParameter ParameterType { get; set; }
    }

    public enum CandleParameter
    {
        Open,
        Close,
        Low,
        High,
        Average,
        Volume,
        TradesNumber
    }
}
