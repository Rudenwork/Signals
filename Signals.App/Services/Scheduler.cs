using MassTransit;
using Quartz;
using Quartz.Impl.Matchers;
using Signals.App.Extensions;
using System.Text.Json;

namespace Signals.App.Services
{
    public class Scheduler
    {
        private IScheduler JobScheduler { get; }

        public Scheduler(ISchedulerFactory schedulerFactory)
        {
            JobScheduler = schedulerFactory.GetScheduler().Result;
        }

        public async Task Publish<TMessage>(TMessage message, DateTime publishAt, Guid? groupId = null)
        {
            await Publish(message, publishAt, cron:null, group: groupId?.ToString());
        }

        public async Task RecurringPublish<TMessage>(TMessage message, string cron, Guid groupId)
        {
            await Publish(message, publishAt: null, cron, groupId.ToString());
        }

        public async Task CancelPublish(Guid groupId)
        {
            CancelPublish(groupId.ToString());
        }

        private async Task Publish<TMessage>(TMessage message, DateTime? publishAt = null, string cron = null, string group = null)
        {
            group = group ?? Guid.NewGuid().ToString();

            var json = JsonSerializer.Serialize(message);
            var type = typeof(TMessage).FullName;
            var name = $"{type}:{Guid.NewGuid()}";

            var job = JobBuilder
                .Create<PublishJob>()
                .WithIdentity(name, group)
                .UsingJobData(PublishJob.MessageType, type)
                .UsingJobData(PublishJob.MessageJson, json)
                .Build();

            var triggerBuilder = TriggerBuilder
                .Create()
                .WithIdentity(name, group);

            if (cron is not null)
            {
                triggerBuilder.WithCronSchedule(cron, x => x.WithMisfireHandlingInstructionDoNothing());
            }
            else if (publishAt is not null)
            {
                triggerBuilder.StartAt(publishAt.Value);
            }
            else
            {
                triggerBuilder.StartNow();
            }

            var trigger = triggerBuilder.Build();

            await JobScheduler.ScheduleJob(job, trigger);
        }

        private async Task CancelPublish(string group)
        {
            var jobKeys = await JobScheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            var triggerKeys = await JobScheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(group));

            await JobScheduler.UnscheduleJobs(triggerKeys);
            await JobScheduler.DeleteJobs(jobKeys);
        }

        private class PublishJob : IJob
        {
            public const string MessageType = nameof(MessageType);
            public const string MessageJson = nameof(MessageJson);

            private IBus Bus { get; }

            public PublishJob(IBus bus)
            {
                Bus = bus;
            }

            public async Task Execute(IJobExecutionContext context)
            {
                var type = Type.GetType(context.MergedJobDataMap[MessageType].ToString());
                var json = context.MergedJobDataMap[MessageJson].ToString();

                var message = JsonSerializer.Deserialize(json, type);
                
                await Bus.Publish(message, context.ScheduledFireTimeUtc?.UtcDateTime);
            }
        }
    }
}
