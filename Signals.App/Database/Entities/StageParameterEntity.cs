using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities
{
    public class StageParameterEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid StageId { get; set; }

        [MaxLength(100)]
        public string Key { get; set; }

        [MaxLength(100)]
        public string Value { get; set; }
    }
}
