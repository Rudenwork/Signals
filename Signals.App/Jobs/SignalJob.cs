using MediatR;
using Quartz;
using Signals.App.Commands;

namespace Signals.App.Jobs
{
    public class SignalJob : IJob
    {
        private IMediator Mediator { get; }

        public SignalJob(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Mediator.Send(new StartSignal.Command
            { 
                SignalId = Guid.Parse(context.JobDetail.Key.Name)
            });
        }
    }
}
