using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Matchers;
using Signals.App.Commands.Signal;
using Signals.App.Core.Test;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Database.Entities.Stages;
using Signals.App.Extensions;
using Signals.App.Identity;
using Signals.App.Services;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = IdentityRoles.Admin)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TestController : ControllerBase
    {
        private SignalsContext SignalsContext { get; }
        private ISchedulerFactory SchedulerFactory { get; }
        private CommandService CommandService { get; }
        private IBus Bus { get; }
        private Scheduler Scheduler { get; }

        public TestController(SignalsContext signalsContext, ISchedulerFactory schedulerFactory, CommandService commandService, IBus bus, Scheduler scheduler)
        {
            SignalsContext = signalsContext;
            SchedulerFactory = schedulerFactory;
            CommandService = commandService;
            Bus = bus;
            Scheduler = scheduler;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Get()
        {
            return Ok();
        }

        [HttpPost("cancelRecurring")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CancelRecurring(Guid groupId)
        {
            await Scheduler.CancelPublish(groupId);

            return Ok();
        }

        [HttpPost("scheduleRecurring")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ScheduleRecurring(string text, string cron, Guid groupId)
        {
            await Scheduler.RecurringPublish(new Test.Message { Text = text }, cron, groupId);

            return Ok();
        }

        [HttpPost("schedule")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Schedule(string text, int delaySeconds)
        {
            await Scheduler.Publish(new Test.Message { Text = text }, DateTime.UtcNow.AddSeconds(delaySeconds));

            return Ok();
        }

        [HttpPost("seed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Seed()
        {
            var signal = new SignalEntity
            {
                UserId = User.GetId(),
                Name = "Test Signal",
                Schedule = "0 0/5 * * * ?"
            };

            SignalsContext.Signals.Add(signal);

            var stage1 = new WaitingStageEntity
            {
                SignalId = signal.Id,
                Type = StageEntity.StageType.Waiting,
                Name = "Test Waiting Stage 1",
                Period = TimeSpan.FromSeconds(10)
            };

            var stage2 = new WaitingStageEntity
            {
                SignalId = signal.Id,
                Type = StageEntity.StageType.Waiting,
                Name = "Test Waiting Stage 2",
                Period = TimeSpan.FromSeconds(5)
            };

            SignalsContext.Stages.Add(stage1);
            SignalsContext.Stages.Add(stage2);

            stage1.NextStageId = stage2.Id;
            stage2.PreviousStageId = stage1.Id;

            await SignalsContext.SaveChangesAsync();

            ///TODO: Change to Scheduler.RecurringPublish(...)
            await CommandService.ScheduleRecurring(new StartSignal.Command { SignalId = signal.Id }, signal.Schedule, signal.Id);

            return Ok();
        }

        [HttpGet("jobs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetJobs()
        {
            var scheduler = await SchedulerFactory.GetScheduler();

            var triggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());

            var jobs = triggerKeys
                .Select(triggerKey => 
                {
                    var trigger = scheduler.GetTrigger(triggerKey).Result;

                    return new
                    {
                        Key = trigger.JobKey,
                        PreviousFireTime = trigger.GetPreviousFireTimeUtc(),
                        NextFireTime = trigger.GetNextFireTimeUtc() 
                    };
                })
                .ToList();

            return Ok(jobs);
        }
    }
}
