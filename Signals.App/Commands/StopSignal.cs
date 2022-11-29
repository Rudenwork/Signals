using MediatR;

namespace Signals.App.Commands
{
    public class StopSignal
    {
        public class Command : IRequest
        {
            public Guid SignalId { get; set; }
        }

        private class Handler : IRequestHandler<Command>
        {
            public Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
