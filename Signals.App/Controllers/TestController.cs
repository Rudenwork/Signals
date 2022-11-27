using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl.Matchers;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Extensions;
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

        [HttpGet()]
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

            var stage1 = new StageEntity
            {
                SignalId = signal.Id,
                Type = StageEntity.StageType.Waiting,
                Name = "Test Waiting Stage 1",
                Parameters = new List<StageParameterEntity>
                {
                    new StageParameterEntity
                    {
                        Key = "Period",
                        Value = TimeSpan.FromSeconds(10).ToString()
                    }
                }
            };

            var stage2 = new StageEntity
            {
                SignalId = signal.Id,
                Type = StageEntity.StageType.Waiting,
                Name = "Test Waiting Stage 2",
                Parameters = new List<StageParameterEntity>
                {
                    new StageParameterEntity
                    {
                        Key = "Period",
                        Value = TimeSpan.FromSeconds(5).ToString()
                    }
                }
            };

            SignalsContext.Stages.Add(stage1);
            SignalsContext.Stages.Add(stage2);

            stage1.NextStageId = stage2.Id;
            stage2.PreviousStageId = stage1.Id;

            await SignalsContext.SaveChangesAsync();

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
