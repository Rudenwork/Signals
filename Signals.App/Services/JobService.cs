using Quartz;
using Quartz.Impl.Matchers;

namespace Signals.App.Services
{
    public class JobService
    {
        private IScheduler Scheduler { get; }

        public JobService(ISchedulerFactory schedulerFactory)
        {
            Scheduler = schedulerFactory.GetScheduler().Result;
        }

        public async Task Schedule<TJob>(Guid id, string cron) where TJob : IJob
        {
            await Schedule<TJob>(id, cron, null);
        }

        public async Task Schedule<TJob>(Guid id, DateTime startAt) where TJob : IJob
        {
            await Schedule<TJob>(id, null, startAt);
        }

        public async Task Schedule<TJob>(Guid id) where TJob : IJob
        {
            await Schedule<TJob>(id, null, null);
        }

        private async Task Schedule<TJob>(Guid id, string cron = null, DateTime? startAt = null) where TJob: IJob
        {
            var jobId = id.ToString();

            var job = JobBuilder
                .Create<TJob>()
                .WithIdentity(jobId, typeof(TJob).Name)
                .Build();

            var triggerBuilder = TriggerBuilder
                .Create()
                .WithIdentity(jobId, typeof(TJob).Name);

            if (cron is not null)
            {
                triggerBuilder.WithCronSchedule(cron);
            }
            else if (startAt is not null)
            {
                triggerBuilder.StartAt(startAt.Value);
            }
            else
            {
                triggerBuilder.StartNow();
            }

            var trigger = triggerBuilder.Build();

            await Scheduler.ScheduleJob(job, trigger);
        }
    }
}
