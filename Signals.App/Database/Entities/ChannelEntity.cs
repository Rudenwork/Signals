using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities
{
    public class ChannelEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "varchar")]
        public ChannelType Type { get; set; }

        public Guid UserId { get; set; }
        public long? ExternalId { get; set; }
        public string? Description { get; set; }
        public string Destination { get; set; }
        public string Code { get; set; }
        public bool IsVerified { get; set; }
    }

    public enum ChannelType
    {
        Telegram,
        Email
    }
}
