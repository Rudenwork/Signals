using MediatR;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Services;

namespace Signals.App.Commands.Signal
{
    public class RescheduleStage
    {
        public class Command : IRequest<Unit>
        {
            public Guid SignalId { get; set; }
            public DateTime ScheduleOn { get; set; }
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

                signalExecution.StageScheduledOn = command.ScheduleOn;
                signalExecution.StageRetryAttempt++;

                SignalsContext.Update(signalExecution);
                SignalsContext.SaveChanges();

                await CommandService.Schedule(new ExecuteStage.Command { SignalId = command.SignalId }, command.ScheduleOn, command.SignalId);

                return Unit.Value;
            }
        }
    }
}
