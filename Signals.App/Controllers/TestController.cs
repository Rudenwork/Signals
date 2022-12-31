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
using Signals.App.Database.Entities.Channels;
using Signals.App.Database.Entities.Indicators;
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
            var channel = new EmailChannelEntity
            {
                UserId = User.GetId(),
                Address = "rudenwork@gmail.com",
                Code = "123456",
                IsVerified = true
            };

            SignalsContext.Channels.Add(channel);

            var signal = new SignalEntity
            {
                UserId = User.GetId(),
                Name = "Test Signal",
                Schedule = "0 0/5 * * * ?",
                Stages = new List<StageEntity>
                {
                    new ConditionStageEntity
                    {
                        Name = "Condition Stage",
                        RetryCount = 3,
                        RetryDelay = TimeSpan.FromSeconds(3),
                        Block = new GroupBlockEntity
                        {
                            Type = GroupBlockType.Or,
                            Children = new List<BlockEntity>
                            {
                                new ValueBlockEntity
                                {
                                    Operator = ValueBlockOperator.GreaterOrEqual,
                                    LeftIndicator = new CandleIndicatorEntity
                                    {
                                        Symbol = "ETHBUSD",
                                        Interval = Interval.OneHour,
                                        ParameterType = CandleParameter.Close
                                    },
                                    RightIndicator = new ConstantIndicatorEntity
                                    {
                                        Value = 1000
                                    }
                                },
                                new GroupBlockEntity
                                {
                                    Type = GroupBlockType.And,
                                    Children = new List<BlockEntity>
                                    {
                                        new ChangeBlockEntity
                                        {
                                            Period = TimeSpan.FromHours(2),
                                            Type = ChangeBlockType.Decrease,
                                            Operator = ChangeBlockOperator.GreaterOrEqual,
                                            Target = 1,
                                            Indicator = new RelativeStrengthIndexIndicatorEntity
                                            {
                                                Symbol = "ETHBUSD",
                                                Interval = Interval.OneHour,
                                                LoopbackPeriod = 14
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new WaitingStageEntity
                    {
                        Name = "Waiting Stage",
                        Period = TimeSpan.FromSeconds(5)
                    },
                    new NotificationStageEntity
                    {
                        Name = "Notification Stage",
                        ChannelId = channel.Id,
                        Message = "Test Message"
                    }
                }
            };
            
            SignalsContext.Signals.Add(signal);

            for (int i = 1; i < signal.Stages.Count; i++)
            {
                signal.Stages[i - 1].NextStageId = signal.Stages[i].Id;
                signal.Stages[i].PreviousStageId = signal.Stages[i - 1].Id;
            }

            await SignalsContext.SaveChangesAsync();

            //await Scheduler.RecurringPublish(new Start.Message { SignalId = signal.Id }, signal.Schedule, signal.Id);

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
