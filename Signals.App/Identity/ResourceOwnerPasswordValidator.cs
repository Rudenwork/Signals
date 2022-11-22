using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using MediatR;
using Signals.App.Commands.User;

namespace Signals.App.Identity
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IMediator Mediator { get; }

        public ResourceOwnerPasswordValidator(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var result = await Mediator.Send(new ValidateCommand.Request 
            { 
                Username = context.UserName, 
                Password = context.Password 
            });

            if (result.Problems is null)
            {
                context.Result = new GrantValidationResult(result.Value.UserId.ToString(), GrantType.ResourceOwnerPassword);
            }
        }
    }
}
