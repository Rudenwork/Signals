using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl.Matchers;
using Signals.App.Controllers.Extensions;
using Signals.App.Core.Jobs;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Identity;

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

        public TestController(SignalsContext signalsContext, ISchedulerFactory schedulerFactory)
        {
            SignalsContext = signalsContext;
            SchedulerFactory = schedulerFactory;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Get()
        {
            var stages = SignalsContext.Stages
                .Include(x => x.Parameters)
                .ToList();

            return Ok(stages);
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

            var stage = new StageEntity
            {
                SignalId = signal.Id,
                Type = StageEntity.StageType.Waiting,
                Name = "Test Waiting Stage",
                Parameters = new List<StageParameterEntity>
                {
                    new StageParameterEntity
                    {
                        Key = "Period",
                        Value = TimeSpan.FromMinutes(5).ToString()
                    }
                }
            };

            SignalsContext.Stages.Add(stage);

            await SignalsContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("schedule")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ScheduleJob()
        {
            var key = Guid.NewGuid();

            var job = JobBuilder
                .Create<TestJob>()
                //.WithIdentity("name")
                .UsingJobData("key", key)
                .Build();

            var trigger = TriggerBuilder
                .Create()
                .StartAt(DateTime.UtcNow.AddSeconds(10))
                //.WithCronSchedule("0 52 18 25 11 ?")
                .Build();

            var scheduler = await SchedulerFactory.GetScheduler();
            await scheduler.ScheduleJob(job, trigger);

            return Ok(new { Key = key });
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
