using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Indicators
{
    public class BollingerBandsIndicatorEntity : IndicatorEntity 
    {
        [Column(TypeName = "nvarchar(25)")]
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
