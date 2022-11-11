using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;
using Signals.App.Database;
using Signals.App.Database.Entities;

namespace Signals.App.Identity
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private SignalsContext SignalsContext { get; set; }
        private IPasswordHasher<UserEntity> PasswordHasher { get; set; }

        public ResourceOwnerPasswordValidator(SignalsContext signalsContext, IPasswordHasher<UserEntity> passwordHasher)
        {
            SignalsContext = signalsContext;
            PasswordHasher = passwordHasher;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = SignalsContext.Users.FirstOrDefault(u => u.Username == context.UserName);

            if (user is null)
                return;

            var verifyResult = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, context.Password);

            if (verifyResult is not PasswordVerificationResult.Failed)
                context.Result = new GrantValidationResult(user.Id.ToString(), GrantType.ResourceOwnerPassword);
        }
    }
}
