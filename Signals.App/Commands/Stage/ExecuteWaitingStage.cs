using MediatR;
using Signals.App.Commands.Signal;
using Signals.App.Services;

namespace Signals.App.Commands.Stage
{
    public class ExecuteWaitingStage
    {
        public class Command : IRequest
        {
            public Guid SignalId { get; set; }
            public bool IsBeginning { get; set; }
            public TimeSpan Period { get; set; }
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
                if (command.IsBeginning)
                {
                    return await CommandService.Execute(new RescheduleStage.Command
                    {
                        SignalId = command.SignalId,
                        ScheduleOn = DateTime.UtcNow + command.Period
                    });
                }

                return await CommandService.Execute(new NextStage.Command { SignalId = command.SignalId });
            }
        }
    }
}
