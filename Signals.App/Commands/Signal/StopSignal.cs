using MediatR;
using Microsoft.EntityFrameworkCore;
using Signals.App.Database;
using Signals.App.Services;

namespace Signals.App.Commands.Signal
{
    public class StopSignal
    {
        public class Command : IRequest<Unit>
        {
            public Guid SignalId { get; set; }
        }

        private class Handler : IRequestHandler<Command>
        {
            private SignalsContext SignalsContext { get; }
            private CommandService CommandService { get; }

            public Handler(SignalsContext signalsContext, CommandService commandService)
            {
                SignalsContext = signalsContext;
                CommandService = commandService;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                SignalsContext.SignalExecutions
                    .Where(x => x.SignalId == command.SignalId)
                    .ExecuteDelete();

                SignalsContext.SaveChanges();

                await CommandService.Unschedule(command.SignalId);

                return Unit.Value;
            }
        }
    }
}
