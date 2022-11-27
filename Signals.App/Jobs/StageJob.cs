using Microsoft.EntityFrameworkCore;
using Quartz;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Services;

namespace Signals.App.Jobs
{
    public class StageJob : IJob
    {
        private SignalsContext SignalsContext { get; }
        private JobService JobService { get; }

        public StageJob(SignalsContext signalsContext, JobService jobService)
        {
            SignalsContext = signalsContext;
            JobService = jobService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var stageExecutionId = Guid.Parse(context.MergedJobDataMap[nameof(StageExecutionEntity.Id)].ToString());

            var stageExecution = await SignalsContext.StageExecutions.FindAsync(stageExecutionId);
            var stage = await SignalsContext.Stages.FindAsync(stageExecution.StageId);

            Console.WriteLine($"[{nameof(StageJob)}] - {stage.Name} ({stageExecution.Id})");

            Next(stage);
        }

        private async Task Next(StageEntity stage)
        {
            SignalsContext.StageExecutions
                .Where(x => x.StageId == stage.Id)
                .ExecuteDelete();

            if (stage.NextStageId != null)
            {
                var stageExecution = new StageExecutionEntity
                {
                    StageId = stage.NextStageId.Value,
                    SignalId = stage.SignalId,
                    ScheduledOn = DateTime.UtcNow.AddMinutes(1)
                };

                SignalsContext.StageExecutions.Add(stageExecution);
                await JobService.ScheduleStageExecution(stageExecution);
            }

            

            SignalsContext.SaveChanges();
        }
    }
}
