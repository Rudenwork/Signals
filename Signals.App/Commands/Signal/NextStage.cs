using MediatR;
using Signals.App.Database;
using Signals.App.Services;

namespace Signals.App.Commands.Signal
{
    public class NextStage
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
                var signalExecution = SignalsContext.SignalExecutions.FirstOrDefault(x => x.SignalId == command.SignalId);

                if (signalExecution is null)
                    return Unit.Value;

                var nextStage = SignalsContext.Stages.FirstOrDefault(x => x.PreviousStageId == signalExecution.StageId);

                if (nextStage is null)
                {
                    return await CommandService.Execute(new StopSignal.Command { SignalId = command.SignalId });
                }

                signalExecution.StageId = nextStage.Id;
                signalExecution.StageRetryAttempt = 0;
                signalExecution.StageScheduledOn = DateTime.UtcNow;

                SignalsContext.Update(signalExecution);
                SignalsContext.SaveChanges();

                await CommandService.Schedule(new ExecuteStage.Command { SignalId = command.SignalId });

                return Unit.Value;
            }
        }
    }
}
