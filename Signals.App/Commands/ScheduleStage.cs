using MediatR;

namespace Signals.App.Commands
{
    public class ScheduleStage
    {
        public class Request : IRequest
        {
            public Guid SignalId { get; set; }
        }

        private class Handler : IRequestHandler<Request>
        {
            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
