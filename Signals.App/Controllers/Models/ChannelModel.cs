using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Signals.App.Controllers.Models
{
    public class ChannelModel
    {
        public class Create 
        {
            [Required]
            public ChannelType Type { get; set; }
            [Required]
            public string Destination { get; set; }
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
            public string? Destination { get; set; }
            public string? Description { get; set; }
        }

        public enum ChannelType
        {
            Email
        }
    }
}
