using MediatR;
using Quartz;
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

        public async Task ScheduleRecurring<TCommand>(TCommand command, string cron, Guid? id = null) where TCommand : IRequest
        {
            await Schedule(command, cron, null, id);
        }

        public async Task Schedule<TCommand>(TCommand command, DateTime? startAt = null, Guid? id = null) where TCommand : IRequest
        {
            await Schedule(command, null, startAt, id);
        }

        public async Task Execute<TCommand>(TCommand command) where TCommand : IRequest
        {
            await Mediator.Send(command);
        }

        private async Task Schedule<TCommand>(TCommand command, string cron = null, DateTime? startAt = null, Guid? id = null) where TCommand : IRequest
        {
            var jobId = (id ?? Guid.NewGuid()).ToString();
            var json = JsonSerializer.Serialize(command);

            var job = JobBuilder
                .Create<CommandJob>()
                .WithIdentity(jobId, typeof(TCommand).FullName)
                .UsingJobData(CommandJob.Json, json)
                .Build();

            var triggerBuilder = TriggerBuilder
                .Create()
                .WithIdentity(jobId, typeof(TCommand).FullName);

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
                var type = Type.GetType(context.JobDetail.Key.Group);

                var command = JsonSerializer.Deserialize(json, type);

                await Mediator.Send(command);
            }
        }
    }
}
