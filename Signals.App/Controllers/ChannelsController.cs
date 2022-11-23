using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Signals.App.Controllers.Models;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Extensions;
using System.Data;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ChannelsController : ControllerBase
    {
        private SignalsContext SignalsContext { get; set; }

        public ChannelsController(SignalsContext signalsContext)
        {
            SignalsContext = signalsContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<ChannelModel.Read>> Get([FromQuery] SubsetModel subset, [FromQuery] ChannelModel.Filter filter)
        {
            var query = SignalsContext.Channels.AsQueryable();

            if (filter.Type is not null)
            {
                var channelType = filter.Type.Adapt<ChannelEntity.ChannelType>();
                query = query.Where(c => c.Type == channelType);
            }

            if (filter.Destination is not null)
                query = query.Where(c => c.Destination.Contains(filter.Destination));

            if (filter.Description is not null)
                query = query.Where(c => c.Description.Contains(filter.Description));

            if (filter.IsVerified is not null)
                query = query.Where(c => c.IsVerified == filter.IsVerified.Value);

            var result = query
                .Where(c => c.UserId == User.GetId())
                .Subset(subset.Offset, subset.Limit)
                .Adapt<List<ChannelModel.Read>>();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ChannelModel.Read> Get(Guid id)
        {
            var entity = SignalsContext.Channels
                .FirstOrDefault(c => c.Id == id);

            if (entity == null)
                return NotFound();

            if (entity.UserId != User.GetId())
                return Forbid();

            var result = entity.Adapt<ChannelModel.Read>();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<ChannelModel.Read> Post(ChannelModel.Create model)
        {
            var entity = model.Adapt<ChannelEntity>();
            entity.UserId = User.GetId();

            SignalsContext.Channels.Add(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<ChannelModel.Read>();

            return Created($"{Request.Scheme}://{Request.Host}{Request.Path}/{result.Id}", result);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ChannelModel.Read> Patch(Guid id, ChannelModel.Update model)
        {
            var entity = SignalsContext.Channels
                .FirstOrDefault(c => c.Id == id);

            if (entity is null)
                return NotFound();

            if (entity.UserId != User.GetId())
                return Forbid();

            model.Adapt(entity);

            SignalsContext.Channels.Update(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<ChannelModel.Read>();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(Guid id)
        {
            var entity = SignalsContext.Channels
                .FirstOrDefault(c => c.Id == id);

            if (entity is null)
                return NotFound();

            if (entity.UserId != User.GetId())
                return Forbid();

            SignalsContext.Channels.Remove(entity);
            SignalsContext.SaveChanges();

            return NoContent();
        }
    }
}
