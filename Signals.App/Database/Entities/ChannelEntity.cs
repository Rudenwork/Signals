using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class ChannelEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public string Code { get; set; }
        public bool IsVerified { get; set; }
    }
}
