using MediatR;
using Quartz;
using Signals.App.Commands;

namespace Signals.App.Jobs
{
    public class StageJob : IJob
    {
        private IMediator Mediator { get; }

        public StageJob(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Mediator.Send(new ExecuteStage.Command
            {
                SignalId = Guid.Parse(context.JobDetail.Key.Name)
            });
        }
    }
}
