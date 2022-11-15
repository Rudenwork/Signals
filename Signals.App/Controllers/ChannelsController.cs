using Duende.IdentityServer.Extensions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Signals.App.Controllers.Models;
using Signals.App.Database;
using Signals.App.Database.Entities;
using System.Data;
using System.Security.Claims;

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

            TypeAdapterConfig<ChannelModel.Update, ChannelEntity>
                .NewConfig()
                .IgnoreNullValues(true);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<ChannelModel.Read>> Get(int? offset = null, int? limit = null)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var query = SignalsContext.Channels.AsQueryable()
                .Where(c => c.UserId == userId);

            if (offset is not null)
                query = query.Skip(offset.Value);

            if (limit is not null)
                query = query.Take(limit.Value);

            var result = query
                .ToList()
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

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (entity.UserId != userId)
                return Forbid();

            var result = entity.Adapt<ChannelModel.Read>();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<ChannelModel.Read> Post(ChannelModel.Create model)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var entity = model.Adapt<ChannelEntity>();
            entity.UserId = userId;

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

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (entity.UserId != userId)
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

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (entity.UserId != userId)
                return Forbid();

            SignalsContext.Channels.Remove(entity);
            SignalsContext.SaveChanges();

            return NoContent();
        }
    }
}
