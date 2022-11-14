using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Signals.App.Controllers.Models;
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
    public class UsersController : ControllerBase
    {
        private SignalsContext SignalsContext { get; set; }
        private IPasswordHasher<UserEntity> PasswordHasher { get; set; }

        public UsersController(SignalsContext context, IPasswordHasher<UserEntity> passwordHasher)
        {
            SignalsContext = context;
            PasswordHasher = passwordHasher;

            TypeAdapterConfig<UserModel.Update, UserEntity>
                .NewConfig()
                .IgnoreNullValues(true);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<UserModel.Read>> Get(int? offset = null, int? limit = null, bool? isDisabled = null, string? username = null)
        {
            var query = SignalsContext.Users.AsQueryable();

            if (offset is not null)
                query = query.Skip(offset.Value);

            if (limit is not null)
                query = query.Take(limit.Value);

            if (isDisabled is not null)
                query = query.Where(u => u.IsDisabled == isDisabled.Value);

            if (username is not null)
                query = query.Where(u => u.Username.Contains(username));

            var result = query
                .ProjectToType<UserModel.Read>()
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserModel.Read> Get(Guid id)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(u => u.Id == id);

            if (entity == null)
                return NotFound();

            var result = entity.Adapt<UserModel.Read>();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<UserModel.Read> Post(UserModel.Create model)
        {
            if (SignalsContext.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Already taken");
                return ValidationProblem();
            }

            var entity = model.Adapt<UserEntity>();
            entity.PasswordHash = PasswordHasher.HashPassword(entity, model.Password);

            SignalsContext.Users.Add(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<UserModel.Read>();

            return Created($"{Request.Scheme}://{Request.Host}{Request.Path}/{result.Id}", result);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserModel.Read> Patch(Guid id, UserModel.Update model)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(u => u.Id == id);

            if (entity is null)
                return NotFound();

            if (SignalsContext.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Already taken");
                return ValidationProblem();
            }

            model.Adapt(entity);

            if (model.Password is not null)
                entity.PasswordHash = PasswordHasher.HashPassword(entity, model.Password);

            SignalsContext.Users.Update(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<UserModel.Read>();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(Guid id)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(u => u.Id == id);

            if (entity is null)
                return NotFound();

            SignalsContext.Users.Remove(entity);
            SignalsContext.SaveChanges();

            return NoContent();
        }
    }
}
