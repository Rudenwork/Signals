using Quartz;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Jobs;

namespace Signals.App.Services
{
    public class JobService
    {
        public SignalsContext SignalsContext { get; }
        public IScheduler Scheduler { get; }

        public JobService(SignalsContext signalsContext, IScheduler scheduler)
        {
            SignalsContext = signalsContext;
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
    }
}
