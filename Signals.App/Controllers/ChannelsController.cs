using Mapster;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Signals.App.Controllers.Models;
using Signals.App.Core.Notification;
using Signals.App.Database;
using Signals.App.Database.Entities;
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
        public ActionResult<List<ChannelModel.Read>> Get([FromQuery] SubsetModel subset, [FromQuery] ChannelModel.Read.Filter filter)
        {
            var query = SignalsContext.Channels.AsQueryable();

            if (filter.Type is not null)
            {
                var type = filter.Type.Adapt<ChannelType>();
                query = query.Where(x => x.Type == type);
            }

            if (filter.Description is not null)
                query = query.Where(x => EF.Functions.ILike(x.Description, $"%{filter.Description}%"));

            if (filter.IsVerified is not null)
                query = query.Where(x => x.IsVerified == filter.IsVerified.Value);

            if (filter.Destination is not null)
                query = query.Where(x => EF.Functions.ILike(x.Destination, $"%{filter.Destination}%"));

            var result = query
                .Where(x => x.UserId == User.GetId())
                .OrderBy(x => x.Destination)
                .Subset(subset.Offset, subset.Limit)
                .Adapt<List<ChannelModel.Read>>();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<ChannelModel.Read> Get(Guid id)
        {
            var entity = SignalsContext.Channels.Find(id);

            if (entity == null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            var result = entity.Adapt<ChannelModel.Read>();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ChannelModel.Read>> Post(ChannelModel.Create model)
        {
            var entity = model.Adapt<ChannelEntity>();

            if (SignalsContext.Channels.Any(x => x.Type == entity.Type && EF.Functions.ILike(x.Destination, entity.Destination)))
            {
                ModelState.AddModelError(nameof(model.Destination), "Already created");
                return ValidationProblem();
            }

            entity.UserId = User.GetId();
            entity.Code = GenerateCode();

            if (entity.Type is ChannelType.Email)
                await SendVerificationEmail(entity);

            SignalsContext.Channels.Add(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<ChannelModel.Read>();

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ChannelModel.Read>> Patch(Guid id, ChannelModel.Update model)
        {
            var entity = SignalsContext.Channels.Find(id);

            if (entity is null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            if (SignalsContext.Channels.Any(x => x.Id != id && x.Type == entity.Type && EF.Functions.ILike(x.Destination, entity.Destination)))
            {
                ModelState.AddModelError(nameof(model.Destination), "Already created");
                return ValidationProblem();
            }

            var shouldReset = model.Type is not null || model.Destination is not null;

            if (shouldReset)
            {
                entity.IsVerified = false;
                entity.ExternalId = null;
                entity.Code = GenerateCode();
            }

            model.Adapt(entity);

            if (shouldReset && model.Type is ChannelModel.TypeEnum.Email)
            {
                await SendVerificationEmail(entity);
            }

            SignalsContext.Channels.Update(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<ChannelModel.Read>();

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

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult<ChannelModel.Read>> Verify(Guid id, ChannelModel.Verify model)
        {
            var entity = SignalsContext.Channels.Find(id);

            if (entity is null)
                return NoContent();

            if (entity.UserId != User.GetId())
                return Forbid();

            if (entity.IsVerified)
            {
                ModelState.AddModelError(nameof(entity.IsVerified), "Already verified");
                return ValidationProblem();
            }

            if (model.Code != entity.Code)
            {
                ModelState.AddModelError(nameof(model.Code), "Invalid");
                return ValidationProblem();
            }

            entity.IsVerified = true;

            SignalsContext.Channels.Update(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<ChannelModel.Read>();

            return Ok(result);
        }

        private static string GenerateCode() => Random.Shared.Next(1000, 10000).ToString();

        private async Task SendVerificationEmail(ChannelEntity entity)
        {
            try
            {
                await Mediator.Send(new SendEmailNotification.Request
                {
                    Address = entity.Destination,
                    Topic = "Signals Verification Code",
                    Text = $"Verification Code: {entity.Code}"
                });
            }
            catch (Exception) { }
        }
    }
}
