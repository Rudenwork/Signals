using MediatR;
using Signals.App.Commands.Signal;
using Signals.App.Services;

namespace Signals.App.Commands.Stage
{
    public class ExecuteConditionStage
    {
        public class Command : IRequest
        {
            public Guid SignalId { get; set; }
            public int RetryAttempt { get; set; }
            public int RetryCount { get; set; }
            public TimeSpan RetryDelay { get; set; }
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
                ///TODO: Condition Stage Logic

                if (false/*condition result is false*/)
                {
                    if (command.RetryCount > command.RetryAttempt)
                    {
                        return await CommandService.Execute(new RescheduleStage.Command
                        {
                            SignalId = command.SignalId,
                            ScheduleOn = DateTime.UtcNow + command.RetryDelay
                        });
                    }

                    return await CommandService.Execute(new StopSignal.Command { SignalId = command.SignalId });
                }

                return await CommandService.Execute(new NextStage.Command { SignalId = command.SignalId });
            }
        }
    }
}
