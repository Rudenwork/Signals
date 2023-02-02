using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Signals.App.Controllers.Models;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Extensions;
using Signals.App.Identity;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = IdentityRoles.Admin)]
    public class UsersController : ControllerBase
    {
        private SignalsContext SignalsContext { get; }
        private IPasswordHasher<UserEntity> PasswordHasher { get; }

        public UsersController(SignalsContext context, IPasswordHasher<UserEntity> passwordHasher)
        {
            SignalsContext = context;
            PasswordHasher = passwordHasher;
        }

        [HttpGet]
        public ActionResult<List<UserModel>> Get([FromQuery] SubsetModel subset, [FromQuery] UserModel.Filter filter)
        {
            var query = SignalsContext.Users.AsQueryable();

            if (filter.Username is not null)
                query = query.Where(x => x.Username.Contains(filter.Username));

            if (filter.IsAdmin is not null)
                query = query.Where(x => x.IsAdmin == filter.IsAdmin);

            if (filter.IsDisabled is not null)
                query = query.Where(x => x.IsDisabled == filter.IsDisabled.Value);

            var result = query
                .Subset(subset.Offset, subset.Limit)
                .Adapt<List<UserModel>>();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<UserModel> Get(Guid id)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return NoContent();

            var result = entity.Adapt<UserModel>();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<UserModel> Post(UserModel.Create model)
        {
            if (SignalsContext.Users.Any(x => x.Username == model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Already taken");
                return ValidationProblem();
            }

            var entity = model.Adapt<UserEntity>();
            entity.PasswordHash = PasswordHasher.HashPassword(entity, model.Password);

            SignalsContext.Users.Add(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<UserModel>();

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public ActionResult<UserModel> Patch(Guid id, UserModel model)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
                return NoContent();

            if (SignalsContext.Users.Any(x => x.Username == model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Already taken");
                return ValidationProblem();
            }

            model.Adapt(entity);

            if (model.Password is not null)
                entity.PasswordHash = PasswordHasher.HashPassword(entity, model.Password);

            SignalsContext.Users.Update(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<UserModel>();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
                return NoContent();

            SignalsContext.Users.Remove(entity);
            SignalsContext.SaveChanges();

            return Ok();
        }
    }
}
