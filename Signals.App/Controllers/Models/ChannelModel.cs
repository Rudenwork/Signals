using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Signals.App.Controllers.Models
{
    public class ChannelModel
    {
        public class Create : IValidatableObject
        {
            [Required]
            public Type? Type { get; set; }

            [Required, MaxLength(100)]
            public string? Destination { get; set; }

            [MaxLength(100)]
            public string? Description { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                switch (Type)
                {
                    case ChannelModel.Type.Email:
                    {
                        if (!Regex.IsMatch(Destination, Constants.Destination.Email.Regex))
                        {
                            yield return new ValidationResult(Constants.Destination.Email.ErrorMessage, new[] { nameof(Destination) });
                        }

                        break;
                    }
                }
            }
        }

        public class Read 
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public Type Type { get; set; }
            public string Destination { get; set; }
            public string Description { get; set; }
            public bool IsVerified { get; set; }
        }

        public class Update : IValidatableObject
        {
            public Type? Type { get; set; }

            [MaxLength(100)]
            public string? Destination { get; set; }

            [MaxLength(100)]
            public string? Description { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Type is null && Destination is not null)
                {
                    yield return new ValidationResult($"Can't be sent without {nameof(Type)}", new[] { nameof(Destination) });
                    yield break;
                }

                if (Destination is null)
                {
                    yield return new ValidationResult($"Should be sent together with {nameof(Type)}", new[] { nameof(Destination) });
                    yield break;
                }

                switch (Type)
                {
                    case ChannelModel.Type.Email:
                    {
                        if (!Regex.IsMatch(Destination, Constants.Destination.Email.Regex))
                        {
                            yield return new ValidationResult(Constants.Destination.Email.ErrorMessage, new[] { nameof(Destination) });
                        }

                        break;
                    }
                }
            }
        }

        public class Filter
        {
            public Type? Type { get; set; }
            public string? Destination { get; set; }
            public string? Description { get; set; }
            public bool? IsVerified { get; set; }
        }

        public enum Type
        {
            Email
        }

        private static class Constants
        {
            public static class Destination
            {
                public static class Email
                {
                    public const string Regex = "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
                    public const string ErrorMessage = "Incorrect format, email address expected";
                }
            }
        }
    }
}
