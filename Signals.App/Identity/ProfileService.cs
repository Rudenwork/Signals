using Duende.IdentityServer;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using MediatR;
using Signals.App.Queries.User;
using System.Security.Claims;

namespace Signals.App.Identity
{
    public class ProfileService : IProfileService
    {
        private IMediator Mediator { get; }

        public ProfileService(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var id = Guid.Parse(context.Subject.GetSubjectId());

            var result = await Mediator.Send(new GetQuery.Request { Id = id });

            var user = result.Value;

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

            var result = await Mediator.Send(new GetQuery.Request { Id = id });

            var user = result.Value;

            context.IsActive = !user.IsDisabled;
        }
    }
}
