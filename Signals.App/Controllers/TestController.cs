using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Matchers;
using Signals.App.Core.Execution;
using Signals.App.Core.Test;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Database.Entities.Blocks;
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
        private IBus Bus { get; }
        private Scheduler Scheduler { get; }

        public TestController(SignalsContext signalsContext, ISchedulerFactory schedulerFactory, IBus bus, Scheduler scheduler)
        {
            SignalsContext = signalsContext;
            SchedulerFactory = schedulerFactory;
            Bus = bus;
            Scheduler = scheduler;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Get()
        {
            var signal = SignalsContext.Signals.FirstOrDefault();

            await Bus.Publish(new Start.Message { SignalId = signal.Id });

            return Ok();
        }

        [HttpPost("cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Cancel(Guid groupId)
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

            var conditionStage = new ConditionStageEntity
            {
                SignalId = signal.Id,
                Name = "Condition Stage",
                RetryCount = 3,
                RetryDelay = TimeSpan.FromSeconds(3)
            };

            var waitingStage = new WaitingStageEntity
            {
                SignalId = signal.Id,
                Name = "Waiting Stage",
                Period = TimeSpan.FromSeconds(5)
            };

            var groupBlock = new GroupBlockEntity
            {
                GroupType = GroupBlockEntity.GroupBlockType.And,
            };

            SignalsContext.Stages.Add(conditionStage);
            SignalsContext.Stages.Add(waitingStage);
            SignalsContext.Blocks.Add(groupBlock);

            conditionStage.NextStageId = waitingStage.Id;
            waitingStage.PreviousStageId = conditionStage.Id;

            groupBlock.StageId = conditionStage.Id;

            await SignalsContext.SaveChangesAsync();

            await Scheduler.RecurringPublish(new Start.Message { SignalId = signal.Id }, signal.Schedule, signal.Id);

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
