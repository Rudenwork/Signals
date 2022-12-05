using MediatR;
using Signals.App.Services;

namespace Signals.App.Commands
{
    public class ExecuteNotificationStage
    {
        public class Command : IRequest
        {
            public Guid SignalId { get; set; }
        }

        private class Handler : IRequestHandler<Command>
        {
            private CommandService CommandService { get; }

            public Handler(CommandService commandService)
            {
                CommandService = commandService;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                ///TODO: Notification Stage Logic

                return await CommandService.Execute(new NextStage.Command { SignalId = command.SignalId });
            }
        }
    }
}
