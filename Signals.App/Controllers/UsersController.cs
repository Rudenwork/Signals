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
        public ActionResult<List<UserModel.Read>> GetAll(int? offset = null, int? limit = null)
        {
            var query = SignalsContext.Users.AsQueryable();

            if (offset is not null)
                query = query.Skip(offset.Value);

            if (limit is not null)
                query = query.Take(limit.Value);

            var result = query
                .ProjectToType<UserModel.Read>()
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<UserModel.Read> Get(Guid id)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(u => u.Id == id);

            if (entity == null)
                return NoContent();

            var result = entity.Adapt<UserModel.Read>();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<UserModel.Read> Create(UserModel.Create model)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(u => u.Username == model.Username);

            if (entity is not null)
                return BadRequest($"{nameof(model.Username)} '{model.Username}' already taken");

            entity = model.Adapt<UserEntity>();
            entity.PasswordHash = PasswordHasher.HashPassword(entity, model.Password);

            SignalsContext.Users.Add(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<UserModel.Read>();

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public ActionResult<UserModel.Read> Update(Guid id, UserModel.Update model)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(u => u.Id == id);

            if (entity is null)
                return NotFound();

            model.Adapt(entity);

            if (model.Password is not null)
                entity.PasswordHash = PasswordHasher.HashPassword(entity, model.Password);

            SignalsContext.Users.Update(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<UserModel.Read>();

            return Accepted(result);
        }

        [HttpDelete("{id}")]
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
