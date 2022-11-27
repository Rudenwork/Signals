using Quartz;
using Signals.App.Core.Jobs;
using Signals.App.Database;
using Signals.App.Database.Entities;

namespace Signals.App.Core.Extensions
{
    public static class JobsExtensions
    {
        public static void ScheduleJobs(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var signalsContext = scope.ServiceProvider.GetService<SignalsContext>();
            var schedulerFactory = scope.ServiceProvider.GetService<ISchedulerFactory>();

            var scheduler = schedulerFactory.GetScheduler().Result;

            var signals = signalsContext.Signals
                .Where(x => !x.IsDisabled)
                .ToList();

            foreach (var signal in signals)
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

                scheduler.ScheduleJob(job, trigger).Wait();
            }
        }
    }
}
