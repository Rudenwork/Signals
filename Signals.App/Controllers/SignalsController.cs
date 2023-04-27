using Mapster;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Signals.App.Controllers.Models;
using Signals.App.Core.Execution;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Database.Entities.Blocks;
using Signals.App.Database.Entities.Indicators;
using Signals.App.Database.Entities.Stages;
using Signals.App.Extensions;
using Signals.App.Services;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SignalsController : ControllerBase
    {
        private SignalsContext SignalsContext { get; }
        private Scheduler Scheduler { get; }
        private IMediator Mediator { get; }

        public SignalsController(SignalsContext signalsContext, Scheduler scheduler, IMediator mediator, IBus bus)
        {
            TypeAdapterConfig<StageModel, StageEntity>
                .NewConfig()
                .Include<StageModel.Condition, ConditionStageEntity>()
                .Include<StageModel.Waiting, WaitingStageEntity>()
                .Include<StageModel.Notification, NotificationStageEntity>();

            TypeAdapterConfig<BlockModel, BlockEntity>
                .NewConfig()
                .Include<BlockModel.Group, GroupBlockEntity>()
                .Include<BlockModel.Change, ChangeBlockEntity>()
                .Include<BlockModel.Value, ValueBlockEntity>();

            TypeAdapterConfig<IndicatorModel, IndicatorEntity>
                .NewConfig()
                .Include<IndicatorModel.BollingerBands, BollingerBandsIndicatorEntity>()
                .Include<IndicatorModel.Candle, CandleIndicatorEntity>()
                .Include<IndicatorModel.Constant, ConstantIndicatorEntity>()
                .Include<IndicatorModel.ExponentialMovingAverage, ExponentialMovingAverageIndicatorEntity>()
                .Include<IndicatorModel.RelativeStrengthIndex, RelativeStrengthIndexIndicatorEntity>()
                .Include<IndicatorModel.SimpleMovingAverage, SimpleMovingAverageIndicatorEntity>();

            TypeAdapterConfig<StageEntity, StageModel>
                .NewConfig()
                .Include<ConditionStageEntity, StageModel.Condition>()
                .Include<WaitingStageEntity, StageModel.Waiting>()
                .Include<NotificationStageEntity, StageModel.Notification>();

            TypeAdapterConfig<BlockEntity, BlockModel>
                .NewConfig()
                .Include<GroupBlockEntity, BlockModel.Group>()
                .Include<ChangeBlockEntity, BlockModel.Change>()
                .Include<ValueBlockEntity, BlockModel.Value>();

            TypeAdapterConfig<IndicatorEntity, IndicatorModel>
                .NewConfig()
                .Include<BollingerBandsIndicatorEntity, IndicatorModel.BollingerBands>()
                .Include<CandleIndicatorEntity, IndicatorModel.Candle>()
                .Include<ConstantIndicatorEntity, IndicatorModel.Constant>()
                .Include<ExponentialMovingAverageIndicatorEntity, IndicatorModel.ExponentialMovingAverage>()
                .Include<RelativeStrengthIndexIndicatorEntity, IndicatorModel.RelativeStrengthIndex>()
                .Include<SimpleMovingAverageIndicatorEntity, IndicatorModel.SimpleMovingAverage>();

            SignalsContext = signalsContext;
            Scheduler = scheduler;
            Mediator = mediator;
        }

        [HttpGet]
        public ActionResult<List<SignalModel.Read>> Get([FromQuery] SubsetModel subset, [FromQuery] SignalModel.Read.Filter filter) 
        {
            var query = SignalsContext.Signals.AsQueryable();
            
            if (filter.Name is not null)
                query = query.Where(x => EF.Functions.ILike(x.Name, $"%{filter.Name}%"));

            if (filter.IsDisabled is not null)
                query = query.Where(x => x.IsDisabled == filter.IsDisabled);

            if (filter.HasSchedule is not null)
                query = query.Where(x => (x.Schedule != null) == filter.HasSchedule);

            if (filter.HasExecution is not null)
                query = query.Where(x => (x.Execution != null) == filter.HasExecution);

            var entities = query
                .Where(x => x.UserId == User.GetId())
                .OrderBy(x => x.Name)
                .Subset(subset.Offset, subset.Limit)
                .ToList();

            entities.ForEach(FillRelatedEntities);

            var result = entities
                .Select(x => x.Adapt<SignalModel.Read>())
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<SignalModel.Read> Get(Guid id)
        {
            var entity = SignalsContext.Signals.Find(id);

            if (entity == null)
                return NoContent();

            if (!User.IsAdmin() && entity.UserId != User.GetId())
                return Forbid();

            FillRelatedEntities(entity);

            var result = entity.Adapt<SignalModel.Read>();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SignalModel.Read>> Post(SignalModel.Create model)
        {
            var userId = User.GetId();

            var channelIds = model.Stages
                .Where(x => x is StageModel.Notification)
                .Cast<StageModel.Notification>()
                .Select(x => x.ChannelId.Value)
                .ToList();

            var validChannelIds = SignalsContext.Channels
                .Where(x => x.UserId == userId && x.IsVerified)
                .Select(x => x.Id)
                .ToList();

            var invalidChannelIds = channelIds
                .Except(validChannelIds)
                .ToList();

            if (invalidChannelIds.Any())
            {
                foreach (var channelId in invalidChannelIds)
                {
                    ModelState.AddModelError(nameof(channelId), $"{channelId} is Invalid");
                }

                return ValidationProblem();
            }

            var entity = model.Adapt<SignalEntity>();

            entity.UserId = userId;

            for (int i = 0; i < entity.Stages.Count; i++)
            {
                entity.Stages[i].Index = i;

                if (entity.Stages[i] is ConditionStageEntity stage && stage.Block is GroupBlockEntity block)
                {
                    for (int j = 0; j < block.Children.Count; j++)
                    {
                        block.Children[j].Index = j;
                    }
                }
            }

            SignalsContext.Signals.Add(entity);
            await SignalsContext.SaveChangesAsync();

            if(!entity.IsDisabled && entity.Schedule is not null)
            {
                await Scheduler.RecurringPublish(new Start.Message { SignalId = entity.Id }, entity.Schedule, entity.Id);
            }

            var result = entity.Adapt<SignalModel.Read>();

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<SignalModel.Read>> Patch(Guid id, SignalModel.Update model)
        {
            var entity = SignalsContext.Signals.Find(id);

            if (entity is null)
                return NoContent();

            if (!User.IsAdmin() && entity.UserId != User.GetId())
                return Forbid();

            if (model.Name is not null)
            {
                entity.Name = model.Name;
            }

            if (model.Schedule is not null)
            {
                if (!entity.IsDisabled && entity.Schedule is not null)
                {
                    await Scheduler.CancelPublish(entity.Id);
                }

                entity.Schedule = model.Schedule == "never" ? null : model.Schedule;

                if (!entity.IsDisabled && entity.Schedule is not null)
                {
                    await Scheduler.RecurringPublish(new Start.Message { SignalId = entity.Id }, entity.Schedule, entity.Id);
                }
            }

            if (model.Stages is not null)
            {
                var execution = SignalsContext.Executions
                    .AsNoTracking()
                    .FirstOrDefault(x => x.SignalId == entity.Id);

                if (execution is not null)
                {
                    await Mediator.Publish(new Stop.Message { ExecutionId = execution.Id });
                }

                var stages = SignalsContext.Stages
                    .Where(x => x.SignalId == entity.Id)
                    .ToList();

                SignalsContext.Stages.RemoveRange(stages);

                stages = model.Stages
                    .Select((x, i) =>
                    {
                        var stage = x.Adapt<StageModel, StageEntity>();

                        stage.SignalId = entity.Id;
                        stage.Index = i;

                        if (stage is ConditionStageEntity conditionStage && conditionStage.Block is GroupBlockEntity block)
                        {
                            for (int j = 0; j < block.Children.Count; j++)
                            {
                                block.Children[j].Index = j;
                            }
                        }

                        return stage;
                    })
                    .ToList();

                SignalsContext.Stages.AddRange(stages);
            }

            SignalsContext.Signals.Update(entity);
            SignalsContext.SaveChanges();

            FillRelatedEntities(entity);

            var result = entity.Adapt<SignalModel.Read>();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var entity = SignalsContext.Signals.Find(id);

            if (entity is null)
                return NoContent();

            if (!User.IsAdmin() && entity.UserId != User.GetId())
                return Forbid();

            if (!entity.IsDisabled && entity.Schedule is not null)
            {
                await Scheduler.CancelPublish(entity.Id);
            }

            var execution = SignalsContext.Executions.FirstOrDefault(x => x.SignalId == entity.Id);

            if (execution is not null)
            {
                await Mediator.Publish(new Stop.Message { ExecutionId = execution.Id });
            }

            SignalsContext.Signals.Remove(entity);
            SignalsContext.SaveChanges();

            return Ok();
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult<SignalModel.Read>> Enable(Guid id)
        {
            var entity = SignalsContext.Signals.Find(id);

            if (entity is null)
                return NoContent();

            if (!User.IsAdmin() && entity.UserId != User.GetId())
                return Forbid();

            if (!entity.IsDisabled)
            {
                ModelState.AddModelError(nameof(entity.IsDisabled), "Already enabled");
                return ValidationProblem();
            }

            if (entity.Schedule is not null) 
            {
                await Scheduler.RecurringPublish(new Start.Message { SignalId = entity.Id }, entity.Schedule, entity.Id);
            }

            entity.IsDisabled = false;

            SignalsContext.Update(entity);
            SignalsContext.SaveChanges();

            FillRelatedEntities(entity);
            var result = entity.Adapt<SignalModel.Read>();

            return Ok(result);
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult<SignalModel.Read>> Disable(Guid id)
        {
            var entity = SignalsContext.Signals.Find(id);

            if (entity is null)
                return NoContent();

            if (!User.IsAdmin() && entity.UserId != User.GetId())
                return Forbid();

            if (entity.IsDisabled)
            {
                ModelState.AddModelError(nameof(entity.IsDisabled), "Already disabled");
                return ValidationProblem();
            }

            if (entity.Schedule is not null)
            {
                await Scheduler.CancelPublish(entity.Id);
            }

            var execution = SignalsContext.Executions.FirstOrDefault(x => x.SignalId == entity.Id);
            
            if (execution is not null)
            {
                await Mediator.Publish(new Stop.Message { ExecutionId = execution.Id });
            }

            entity.IsDisabled = true;

            SignalsContext.Update(entity);
            SignalsContext.SaveChanges();

            FillRelatedEntities(entity);
            var result = entity.Adapt<SignalModel.Read>();

            return Ok(result);
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult<SignalModel.Read>> Start(Guid id)
        {
            var entity = SignalsContext.Signals.Find(id);

            if (entity is null)
                return NoContent();

            if (!User.IsAdmin() && entity.UserId != User.GetId())
                return Forbid();

            if (entity.IsDisabled)
            {
                ModelState.AddModelError(nameof(entity.IsDisabled), "Signal should be enabled");
                return ValidationProblem();
            }

            var execution = SignalsContext.Executions.FirstOrDefault(x => x.SignalId == entity.Id);

            if (execution is not null)
            {
                ModelState.AddModelError(nameof(execution), "Already started");
                return ValidationProblem();
            }

            await Mediator.Publish(new Start.Message { SignalId = entity.Id });

            FillRelatedEntities(entity);
            var result = entity.Adapt<SignalModel.Read>();

            return Ok(result);
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult<SignalModel.Read>> Stop(Guid id)
        {
            var entity = SignalsContext.Signals.Find(id);

            if (entity is null)
                return NoContent();

            if (!User.IsAdmin() && entity.UserId != User.GetId())
                return Forbid();

            var execution = SignalsContext.Executions.FirstOrDefault(x => x.SignalId == entity.Id);

            if (execution is null)
            {
                ModelState.AddModelError(nameof(execution), "Already stopped");
                return ValidationProblem();
            }

            await Mediator.Publish(new Stop.Message { ExecutionId = execution.Id });

            FillRelatedEntities(entity);
            var result = entity.Adapt<SignalModel.Read>();

            return Ok(result);
        }

        private void FillRelatedEntities(SignalEntity signal)
        {
            if (signal is null)
                return;

            signal.Execution = SignalsContext.Executions.FirstOrDefault(x => x.SignalId == signal.Id);

            FillRelatedEntities(signal.Execution);

            signal.Stages = SignalsContext.Stages
                .Where(x => x.SignalId == signal.Id)
                .OrderBy(x => x.Index)
                .ToList();

            signal.Stages.ForEach(FillRelatedEntities);
        }

        private void FillRelatedEntities(ExecutionEntity execution)
        {
            if (execution is null)
                return;

            execution.Stage = SignalsContext.Stages.FirstOrDefault(x => x.Id == execution.StageId);
        }

        private void FillRelatedEntities(StageEntity stage)
        {
            if (stage is null || stage is not ConditionStageEntity conditionStage)
                return;

            conditionStage.Block = SignalsContext.Blocks.Find(conditionStage.BlockId);
            FillRelatedEntities(conditionStage.Block);
        }

        private void FillRelatedEntities(BlockEntity block)
        {
            if (block is null)
                return;

            if (block is ValueBlockEntity valueBlock)
            {
                valueBlock.LeftIndicator = SignalsContext.Indicators.Find(valueBlock.LeftIndicatorId);
                valueBlock.RightIndicator = SignalsContext.Indicators.Find(valueBlock.RightIndicatorId);
            }

            if (block is ChangeBlockEntity changeBlock)
            {
                changeBlock.Indicator = SignalsContext.Indicators.Find(changeBlock.IndicatorId);
            }

            if (block is GroupBlockEntity groupBlock)
            {
                groupBlock.Children = SignalsContext.Blocks
                    .Where(x => x.ParentBlockId == block.Id)
                    .OrderBy(x => x.Index)
                    .ToList();

                groupBlock.Children.ForEach(FillRelatedEntities);
            }
        }
    }
}
