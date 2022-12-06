using MediatR;
using Signals.App.Commands.Block;
using Signals.App.Commands.Signal;
using Signals.App.Database;
using Signals.App.Services;

namespace Signals.App.Commands.Stage
{
    public class ExecuteConditionStage
    {
        public class Command : IRequest<Unit>
        {
            public Guid SignalId { get; set; }
            public Guid StageId { get; set; }
            public int RetryAttempt { get; set; }
            public int RetryCount { get; set; }
            public TimeSpan RetryDelay { get; set; }
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
                var rootBlock = SignalsContext.Blocks
                    .Where(x => x.StageId == command.StageId && x.ParentBlockId == null)
                    .FirstOrDefault();

                var isBlockSucceded = await CommandService.Execute(new EvaluateBlock.Command { BlockId = rootBlock.Id });

                if (isBlockSucceded)
                {
                    return await CommandService.Execute(new NextStage.Command { SignalId = command.SignalId });
                }

                if (command.RetryAttempt >= command.RetryCount)
                {
                    return await CommandService.Execute(new StopSignal.Command { SignalId = command.SignalId });
                }

                return await CommandService.Execute(new RescheduleStage.Command
                {
                    SignalId = command.SignalId,
                    ScheduleOn = DateTime.UtcNow + command.RetryDelay
                });
            }
        }
    }
}
