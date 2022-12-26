namespace Signals.App.Database.Entities.Indicators
{
    public class BollingerBandsIndicatorEntity : IndicatorEntity 
    {
        public BollingerBand BandType { get; set; }
    }

    ///TODO: Change other entity enums to be external (outside entity)
    public enum BollingerBand
    {
        Lower,
        Middle,
        Upper
    }
}
