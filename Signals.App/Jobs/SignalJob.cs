using Quartz;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Services;

namespace Signals.App.Jobs
{
    public class SignalJob : IJob
    {
        private SignalsContext SignalsContext { get; }
        private JobService JobService { get; }

        public SignalJob(SignalsContext signalsContext, JobService jobService)
        {
            SignalsContext = signalsContext;
            JobService = jobService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var signalId = Guid.Parse(context.MergedJobDataMap[nameof(SignalEntity.Id)].ToString());
            Console.WriteLine($"[{nameof(SignalJob)}] - {signalId}");

            var stage = SignalsContext.Stages.First(x => x.PreviousStageId == null);

            var stageExecution = new StageExecutionEntity
            {
                StageId = stage.Id,
                SignalId = signalId,
                ScheduledOn = DateTime.UtcNow
            };

            SignalsContext.StageExecutions.Add(stageExecution);
            SignalsContext.SaveChanges();

            await JobService.ScheduleStageExecution(stageExecution);
        }
    }
}
