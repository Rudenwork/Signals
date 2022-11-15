using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities
{
    public class ChannelEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public ChannelType Type { get; set; }

        [MaxLength(100)]
        public string Destination { get; set; }

        public bool IsVerified { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public enum ChannelType 
        {
            Email
        }
    }
}
