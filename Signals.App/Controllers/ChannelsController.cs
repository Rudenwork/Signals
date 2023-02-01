using Mapster;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Signals.App.Controllers.Models;
using Signals.App.Core.Notification;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Database.Entities.Channels;
using Signals.App.Extensions;
using System.Data;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChannelsController : ControllerBase
    {
        private SignalsContext SignalsContext { get; }
        private IMediator Mediator { get; }

        public ChannelsController(SignalsContext signalsContext, IMediator mediator)
        {
            SignalsContext = signalsContext;
            Mediator = mediator;
        }

        [HttpGet]
        public ActionResult<List<ChannelModel>> Get([FromQuery] SubsetModel subset, [FromQuery] ChannelModel.Filter filter)
        {
            var query = SignalsContext.Channels.AsQueryable();

            if (filter.Type is not null)
            {
                query = filter.Type switch
                {
                    ChannelModel.Filter.TypeEnum.Email => query.Where(x => x is EmailChannelEntity),
                    ChannelModel.Filter.TypeEnum.Telegram => query.Where(x => x is TelegramChannelEntity)
                };
            }

            if (filter.Description is not null)
                query = query.Where(x => x.Description.Contains(filter.Description));

            if (filter.IsVerified is not null)
                query = query.Where(x => x.IsVerified == filter.IsVerified.Value);

            if (filter.Address is not null)
                query = query.Where(x => (x as EmailChannelEntity).Address.Contains(filter.Address));

            if (filter.Username is not null)
                query = query.Where(x => (x as TelegramChannelEntity).Username.Contains(filter.Username));

            var result = query
                .Where(x => x.UserId == User.GetId())
                .Subset(subset.Offset, subset.Limit)
                .Select(x => AdaptToModel(x))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<ChannelModel> Get(Guid id)
        {
            var entity = SignalsContext.Channels.Find(id);

            if (entity == null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            var result = AdaptToModel(entity);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ChannelModel>> Post(ChannelModel model)
        {
            ChannelEntity entity = model switch
            {
                ChannelModel.Email => model.Adapt<EmailChannelEntity>(),
                ChannelModel.Telegram => model.Adapt<TelegramChannelEntity>()
            };

            entity.UserId = User.GetId();
            entity.Code = GenerateCode();

            if (model is ChannelModel.Email)
                await SendVerificationEmail(entity as EmailChannelEntity);

            SignalsContext.Channels.Add(entity);
            SignalsContext.SaveChanges();

            var result = AdaptToModel(entity);

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ChannelModel>> Patch(Guid id, ChannelModel model)
        {
            var entity = SignalsContext.Channels.Find(id);

            if (entity is null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            var patchEntityType = model switch
            {
                ChannelModel.Email => typeof(EmailChannelEntity),
                ChannelModel.Telegram => typeof(TelegramChannelEntity)
            };

            if (entity.GetType() != patchEntityType)
            {
                ModelState.AddModelError(nameof(model), "Type is not correct");
                return ValidationProblem();
            }

            var shouldReset = model switch
            {
                ChannelModel.Email emailModel => emailModel.Address is not null,
                ChannelModel.Telegram telegramModel => telegramModel.Username is not null
            };

            if ((model as ChannelModel.Email)?.Address is not null || (model as ChannelModel.Telegram)?.Username is not null)
            {
                entity.IsVerified = false;
                entity.Code = GenerateCode();
            }

            if (shouldReset && model is ChannelModel.Email)
                await SendVerificationEmail(entity as EmailChannelEntity);

            model.Adapt(entity, model.GetType(), entity.GetType());

            SignalsContext.Channels.Update(entity);
            SignalsContext.SaveChanges();

            var result = AdaptToModel(entity);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var entity = SignalsContext.Channels.Find(id);

            if (entity is null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            SignalsContext.Channels.Remove(entity);
            SignalsContext.SaveChanges();

            return Ok();
        }

        private static string GenerateCode() => Random.Shared.Next(1000, 10000).ToString();

        private static ChannelModel AdaptToModel(ChannelEntity entity) => entity switch
        {
            EmailChannelEntity => entity.Adapt<ChannelModel.Email>(),
            TelegramChannelEntity => entity.Adapt<ChannelModel.Telegram>()
        };

        private async Task SendVerificationEmail(EmailChannelEntity entity)
        {
            await Mediator.Send(new SendEmailNotification.Request
            {
                Channel = entity,
                Topic = "Signals Verification Code",
                Text = $"Verification Code: {entity.Code}"
            });
        }
    }
}
