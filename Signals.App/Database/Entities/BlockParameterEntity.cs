using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities
{
    public class BlockParameterEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid BlockId { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public ParameterKey Key { get; set; }

        [MaxLength(100)]
        public string Value { get; set; }

        public enum ParameterKey
        {
            GroupType
            ///TODO: Define all known block parameters
        }
    }
}
