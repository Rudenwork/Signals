using Quartz;
using Signals.App.Database;
using Signals.App.Database.Entities;

namespace Signals.App.Jobs
{
    public class StageExecutionJob : IJob
    {
        private SignalsContext SignalsContext { get; }

        public StageExecutionJob(SignalsContext signalsContext)
        {
            SignalsContext = signalsContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var stageExecutionId = Guid.Parse(context.MergedJobDataMap[nameof(StageExecutionEntity.Id)].ToString());

            var stageExecution = await SignalsContext.StageExecutions.FindAsync(stageExecutionId);
            var stage = await SignalsContext.Stages.FindAsync(stageExecution.StageId);

            Console.WriteLine($"[{nameof(StageExecutionJob)}] - {stage.Name} ({stageExecution.Id})");
        }
    }
}
