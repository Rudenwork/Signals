using MediatR;
using Microsoft.AspNetCore.Identity;
using Signals.App.Database.Entities;
using Signals.App.Database;
using Signals.App.Models;
using Mapster;

namespace Signals.App.Queries.User
{
    public class GetQuery
    {
        public class Request : IRequest<Result<Response>>
        {
            public Guid Id { get; set; }
        }

        public class Response : UserModel.Read { }

        private class Handler : IRequestHandler<Request, Result<Response>>
        {
            private SignalsContext SignalsContext { get; }

            public Handler(SignalsContext signalsContext)
            {
                SignalsContext = signalsContext;
            }

            public async Task<Result<Response>> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = SignalsContext.Users.FirstOrDefault(u => u.Id == request.Id);

                if (user is null)
                {
                    return new ResultProblem(nameof(user), "Not Found");
                }

                return user.Adapt<Response>();
            }
        }
    }
}
