using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Matchers;
using Signals.App.Core.Jobs;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TestController : ControllerBase
    {
        private ISchedulerFactory SchedulerFactory { get; }

        public TestController(ISchedulerFactory schedulerFactory)
        {
            SchedulerFactory = schedulerFactory;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Get()
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

            return Ok(new 
            { 
                Jobs = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()),
                Triggers = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup())
            });
        }
    }
}
