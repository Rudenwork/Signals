using Mapster;
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

        public SignalsController(SignalsContext signalsContext, Scheduler scheduler, IMediator mediator)
        {
            SignalsContext = signalsContext;
            Scheduler = scheduler;
            Mediator = mediator;

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
        }

        [HttpGet]
        public ActionResult<List<SignalModel.Read>> Get([FromQuery] SubsetModel subset, [FromQuery] SignalModel.Read.Filter filter) 
        {
            var query = SignalsContext.Signals.AsQueryable();

            var entities = query
                .Where(x => x.UserId == User.GetId())
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

            if (entity.UserId != User.GetId())
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

            SignalsContext.Signals.Add(entity);

            for (int i = 1; i < entity.Stages.Count; i++)
            {
                entity.Stages[i - 1].NextStageId = entity.Stages[i].Id;
                entity.Stages[i].PreviousStageId = entity.Stages[i - 1].Id;
            }

            await SignalsContext.SaveChangesAsync();

            if(!entity.IsDisabled)
            {
                await Scheduler.RecurringPublish(new Start.Message { SignalId = entity.Id }, entity.Schedule, entity.Id);
            }

            var result = entity.Adapt<SignalModel.Read>();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SignalModel>> Delete(Guid id)
        {
            var entity = SignalsContext.Signals.Find(id);

            if (entity is null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            if (!entity.IsDisabled)
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

        private void FillRelatedEntities(SignalEntity signal)
        {
            signal.Stages = SignalsContext.Stages
                .Where(x => x.SignalId == signal.Id)
                .ToList();

            signal.Stages.ForEach(FillRelatedEntities);
        }

        private void FillRelatedEntities(StageEntity stage)
        {
            if (stage is not ConditionStageEntity conditionStage)
                return;

            conditionStage.Block = SignalsContext.Blocks.Find(conditionStage.BlockId);
            FillRelatedEntities(conditionStage.Block);
        }

        private void FillRelatedEntities(BlockEntity block)
        {
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
                    .ToList();

                groupBlock.Children.ForEach(FillRelatedEntities);
            }
        }
    }
}
