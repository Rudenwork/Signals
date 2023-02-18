using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Indicators
{
    public class BollingerBandsIndicatorEntity : IndicatorEntity 
    {
        [Column(TypeName = "varchar")]
        public BollingerBand BandType { get; set; }
    }

    public enum BollingerBand
    {
        Lower,
        Middle,
        Upper
    }
}
