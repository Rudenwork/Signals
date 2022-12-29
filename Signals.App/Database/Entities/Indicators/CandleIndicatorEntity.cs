﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Indicators
{
    public class CandleIndicatorEntity : IndicatorEntity
    {
        [Column(TypeName = "nvarchar(25)")]
        public CandleParameter ParameterType { get; set; }
    }

    public enum CandleParameter
    {
        Open,
        Close,
        Low,
        High,
        Average,
        Volume
    }
}
