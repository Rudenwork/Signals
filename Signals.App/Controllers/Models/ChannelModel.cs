using System.ComponentModel.DataAnnotations;

namespace Signals.App.Controllers.Models
{
    public class ChannelModel
    {
        public class Create 
        {
            [Required]
            public ChannelType Type { get; set; }

            [Required, MaxLength(100)]
            public string Destination { get; set; }

            [MaxLength(100)]
            public string? Description { get; set; }
        }

        public class Read 
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public ChannelType Type { get; set; }
            public string Destination { get; set; }
            public string? Description { get; set; }
            public bool IsVerified { get; set; }
        }

        public class Update 
        {
            public ChannelType? Type { get; set; }

            [MaxLength(100)]
            public string? Destination { get; set; }

            [MaxLength(100)]
            public string? Description { get; set; }
        }

        public class Filter
        {
            public ChannelType? Type { get; set; }
            public string? Destination { get; set; }
            public string? Description { get; set; }
            public bool? IsVerified { get; set; }
        }

        public enum ChannelType
        {
            Email
        }
    }
}
