using MediatR;

namespace Signals.App.Commands
{
    public class StartSignal
    {
        public class Command : IRequest
        {
            public Guid SignalId { get; set; }
        }

        private class Handler : IRequestHandler<Command>
        {
            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                Console.WriteLine($"[{nameof(Command)}] - [{nameof(StartSignal)}] - [{command.SignalId}]");

                return Unit.Value;
            }
        }
    }
}
