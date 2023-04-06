using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private SignalsController SignalsController { get; }

        public UsersController(SignalsContext context, IPasswordHasher<UserEntity> passwordHasher, SignalsController signalsController)
        {
            SignalsContext = context;
            PasswordHasher = passwordHasher;
            SignalsController = signalsController;
        }

        [HttpGet]
        public ActionResult<List<UserModel.Read>> Get([FromQuery] SubsetModel subset, [FromQuery] UserModel.Read.Filter filter)
        {
            var query = SignalsContext.Users.AsQueryable();
            
            if (filter.Username is not null)
                query = query.Where(x => EF.Functions.ILike(x.Username, $"%{filter.Username}%"));

            if (filter.IsAdmin is not null)
                query = query.Where(x => x.IsAdmin == filter.IsAdmin);

            if (filter.IsDisabled is not null)
                query = query.Where(x => x.IsDisabled == filter.IsDisabled.Value);

            var result = query
                .Subset(subset.Offset, subset.Limit)
                .Adapt<List<UserModel.Read>>();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<UserModel.Read> Get(Guid id)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return NoContent();

            var result = entity.Adapt<UserModel.Read>();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<UserModel.Read> Post(UserModel.Create model)
        {
            if (SignalsContext.Users.Any(x => EF.Functions.ILike(x.Username, $"{model.Username}")))
            {
                ModelState.AddModelError(nameof(model.Username), "Already taken");
                return ValidationProblem();
            }

            var entity = model.Adapt<UserEntity>();
            entity.PasswordHash = PasswordHasher.HashPassword(entity, model.Password);

            SignalsContext.Users.Add(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<UserModel.Read>();

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public ActionResult<UserModel.Read> Patch(Guid id, UserModel.Update model)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
                return NoContent();

            if (SignalsContext.Users.Any(x => EF.Functions.ILike(x.Username, $"{model.Username}")))
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
        public async Task<ActionResult> Delete(Guid id)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
                return NoContent();

            var signalIds = SignalsContext.Signals
                .Where(x => x.UserId == id && !x.IsDisabled)
                .Select(x => x.Id)
                .ToList();

            foreach (var signalId in signalIds)
            {
                await SignalsController.Disable(signalId); 
            }

            SignalsContext.Users.Remove(entity);
            SignalsContext.SaveChanges();

            return Ok();
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult<UserModel.Read>> Enable(Guid id)
        {
            var entity = SignalsContext.Users.Find(id);

            if (entity is null)
                return NoContent();

            if (!entity.IsDisabled)
            {
                ModelState.AddModelError(nameof(entity.IsDisabled), "Already enabled");
                return ValidationProblem();
            }

            entity.IsDisabled = false;

            SignalsContext.Update(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<UserModel.Read>();

            return Ok(result);
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult<UserModel.Read>> Disable(Guid id)
        {
            var entity = SignalsContext.Users.Find(id);

            if (entity is null)
                return NoContent();

            if (entity.IsDisabled)
            {
                ModelState.AddModelError(nameof(entity.IsDisabled), "Already disabled");
                return ValidationProblem();
            }

            var signalIds = SignalsContext.Signals
                .Where(x => x.UserId == id && !x.IsDisabled)
                .Select(x => x.Id)
                .ToList();

            foreach (var signalId in signalIds)
            {
                await SignalsController.Disable(signalId);
            }

            entity.IsDisabled = true;

            SignalsContext.Update(entity);
            SignalsContext.SaveChanges();

            var result = entity.Adapt<UserModel.Read>();

            return Ok(result);
        }
    }
}
