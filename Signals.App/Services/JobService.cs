using Quartz;
using Signals.App.Database.Entities;
using Signals.App.Jobs;

namespace Signals.App.Services
{
    public class JobService
    {
        public IScheduler Scheduler { get; }

        public JobService(IScheduler scheduler)
        {
            Scheduler = scheduler;
        }

        public async Task ScheduleSignals(List<SignalEntity> signals)
        {
            foreach (var signal in signals)
            {
                await ScheduleSignal(signal);
            }
        }

        public async Task ScheduleSignal(SignalEntity signal)
        {
            var signalId = signal.Id.ToString();

            var job = JobBuilder
                .Create<SignalJob>()
                .WithIdentity(signalId)
                .UsingJobData(nameof(SignalEntity.Id), signalId)
                .Build();

            var trigger = TriggerBuilder
                .Create()
                .WithIdentity(signalId)
                .WithCronSchedule(signal.Schedule)
                .Build();

            await Scheduler.ScheduleJob(job, trigger);
        }

        public async Task ScheduleStageExecutions(List<StageExecutionEntity> stageExecutions)
        {
            foreach (var stageExecution in stageExecutions)
            {
                await ScheduleStageExecution(stageExecution);
            }
        }

        public async Task ScheduleStageExecution(StageExecutionEntity stageExecution)
        {
            var stageExecutionId = stageExecution.Id.ToString();

            var job = JobBuilder
                .Create<StageJob>()
                .WithIdentity(stageExecutionId)
                .UsingJobData(nameof(StageExecutionEntity.Id), stageExecutionId)
                .Build();

            var trigger = TriggerBuilder
                .Create()
                .WithIdentity(stageExecutionId)
                .StartAt(stageExecution.ScheduledOn)
                .Build();

            await Scheduler.ScheduleJob(job, trigger);
        }
    }
}
