using Duende.IdentityServer;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Signals.App.Database;
using System.Security.Claims;

namespace Signals.App.Identity
{
    public class ProfileService : IProfileService
    {
        private SignalsContext SignalsContext { get; set; }

        public ProfileService(SignalsContext context)
        {
            SignalsContext = context;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var id = Guid.Parse(context.Subject.GetSubjectId());

            var user = SignalsContext.Users
                .FirstOrDefault(u => u.Id == id);

            if (user.IsAdmin)
            {
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, IdentityRoles.Admin));
            }

            var scopes = context.RequestedResources.RawScopeValues.ToList();

            if (scopes.Contains(IdentityServerConstants.StandardScopes.Profile))
            {
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.PreferredUserName, user.Username));
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var id = Guid.Parse(context.Subject.GetSubjectId());

            var user = SignalsContext.Users
                .FirstOrDefault(u => u.Id == id);

            context.IsActive = !user.IsDisabled;
        }
    }
}
