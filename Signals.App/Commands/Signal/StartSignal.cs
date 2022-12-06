using MediatR;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Services;

namespace Signals.App.Commands.Signal
{
    public class StartSignal
    {
        public class Command : IRequest
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
                if (SignalsContext.SignalExecutions.Any(x => x.SignalId == command.SignalId))
                    return Unit.Value;

                var firstStage = SignalsContext.Stages.FirstOrDefault(x => x.SignalId == command.SignalId && x.PreviousStageId == null);

                SignalsContext.SignalExecutions.Add(new SignalExecutionEntity
                {
                    SignalId = command.SignalId,
                    StageId = firstStage.Id,
                    StageScheduledOn = DateTime.UtcNow
                });

                SignalsContext.SaveChanges();

                await CommandService.Schedule(new ExecuteStage.Command { SignalId = command.SignalId });

                return Unit.Value;
            }
        }
    }
}
