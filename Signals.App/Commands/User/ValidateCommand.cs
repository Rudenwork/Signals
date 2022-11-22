using MediatR;
using Microsoft.AspNetCore.Identity;
using Signals.App.Database.Entities;
using Signals.App.Database;

namespace Signals.App.Commands.User
{
    public class ValidateCommand
    {
        public class Request : IRequest<Result<Response>>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Response
        {
            public Guid UserId { get; set; }
        }

        private class Handler : IRequestHandler<Request, Result<Response>>
        {
            private SignalsContext SignalsContext { get; }
            private IPasswordHasher<UserEntity> PasswordHasher { get; }

            public Handler(SignalsContext signalsContext, IPasswordHasher<UserEntity> passwordHasher)
            {
                SignalsContext = signalsContext;
                PasswordHasher = passwordHasher;
            }

            public async Task<Result<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = SignalsContext.Users.FirstOrDefault(u => u.Username == request.Username);

                if (user is null)
                {
                    return new ResultProblem(nameof(user), "Not Found");
                }

                var verifyResult = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

                if (verifyResult is PasswordVerificationResult.Failed)
                {
                    return new ResultProblem(nameof(PasswordVerificationResult), nameof(PasswordVerificationResult.Failed));
                }

                return new Response { UserId = user.Id };
            }
        }
    }
}
