using Azure;
using MediatR;
using Quartz;
using Quartz.Impl.Matchers;
using System.Text.Json;

namespace Signals.App.Services
{
    public class CommandService
    {
        private IScheduler Scheduler { get; }
        private IMediator Mediator { get; }

        public CommandService(ISchedulerFactory schedulerFactory, IMediator mediator)
        {
            Scheduler = schedulerFactory.GetScheduler().Result;
            Mediator = mediator;
        }

        public async Task ScheduleRecurring<TResponse>(IRequest<TResponse> command, string cron, Guid groupId)
        {
            await Schedule(command, cron, null, $"R:{groupId}");
        }

        public async Task Schedule<TResponse>(IRequest<TResponse> command, DateTime? startAt = null, Guid? groupId = null)
        {
            await Schedule(command, null, startAt, groupId?.ToString());
        }

        public async Task<TResponse> Execute<TResponse>(IRequest<TResponse> command)
        {
            return await Mediator.Send(command);
        }

        public async Task UnscheduleRecurring(Guid groupId)
        {
            Unschedule($"R:{groupId}");
        }

        public async Task Unschedule(Guid groupId)
        {
            Unschedule(groupId.ToString());
        }

        private async Task Unschedule(string group)
        {
            var jobKeys = await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            var triggerKeys = await Scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(group));

            await Scheduler.UnscheduleJobs(triggerKeys);
            await Scheduler.DeleteJobs(jobKeys);
        }

        private async Task Schedule<TResponse>(IRequest<TResponse> command, string cron = null, DateTime? startAt = null, string group = null)
        {
            group = group ?? Guid.NewGuid().ToString();
            var json = JsonSerializer.Serialize(command);

            var job = JobBuilder
                .Create<CommandJob>()
                .WithIdentity(command.GetType().FullName, group)
                .UsingJobData(CommandJob.Json, json)
                .Build();

            var triggerBuilder = TriggerBuilder
                .Create()
                .WithIdentity(command.GetType().FullName, group);

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

        private class CommandJob : IJob
        {
            public const string Json = nameof(Json);

            private IMediator Mediator { get; }

            public CommandJob(IMediator mediator)
            {
                Mediator = mediator;
            }

            public async Task Execute(IJobExecutionContext context)
            {
                var json = context.MergedJobDataMap[Json].ToString();
                var type = Type.GetType(context.JobDetail.Key.Name);

                var command = JsonSerializer.Deserialize(json, type);

                await Mediator.Send(command);
            }
        }
    }
}
