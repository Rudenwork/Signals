using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Signals.App.Database;
using Signals.App.Database.Entities;
using Signals.App.Identity;
using Signals.App.Models;
using System.Net;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = IdentityRoles.Admin)]
    public class UsersController : ControllerBase
    {
        private SignalsContext SignalsContext { get; set; }

        public UsersController(SignalsContext context)
        {
            SignalsContext = context;
        }

        [HttpGet]
        public List<UserModel.Read> GetAll()
        {
            return SignalsContext.Users
                .Select(e => new UserModel.Read
                {
                    Id = e.Id,
                    Username = e.Username,
                    IsAdmin = e.IsAdmin,
                    IsDisabled = e.IsDisabled
                })
                .ToList();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(u => u.Id == id);

            if (entity == null)
                return NoContent();

            return Ok(new UserModel.Read
            {
                Id = entity.Id,
                Username = entity.Username,
                IsAdmin = entity.IsAdmin,
                IsDisabled = entity.IsDisabled
            });
        }

        [HttpPost]
        public IActionResult Create(UserModel.Create model)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(u => u.Username == model.Username);

            if (entity is not null)
                return BadRequest($"{nameof(model.Username)} '{model.Username}' already taken");

            var hasher = new PasswordHasher<UserModel.Create>();
            var passwordHash = hasher.HashPassword(model, model.Password);

            entity = new UserEntity
            {
                Username = model.Username,
                PasswordHash = passwordHash,
                IsAdmin = model.IsAdmin.HasValue ? model.IsAdmin.Value : false,
                IsDisabled = model.IsDisabled.HasValue ? model.IsDisabled.Value : false,
            };

            SignalsContext.Users.Add(entity);
            SignalsContext.SaveChanges();

            return Ok(new UserModel.Read
            {
                Id = entity.Id,
                Username = entity.Username,
                IsAdmin = entity.IsAdmin,
                IsDisabled = entity.IsDisabled
            });
        }

        [HttpPatch("{id}")]
        public IActionResult Update(Guid id, UserModel.Update model)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(u => u.Id == id);

            if (entity is null)
                return NotFound();

            if (model.Username is not null)
                entity.Username = model.Username;

            if (model.Password is not null)
            {
                var hasher = new PasswordHasher<UserModel.Update>();
                var passwordHash = hasher.HashPassword(model, model.Password);

                entity.PasswordHash = passwordHash;
            }

            if (model.IsAdmin is not null)
                entity.IsAdmin = model.IsAdmin.Value;

            if (model.IsDisabled is not null)
                entity.IsDisabled = model.IsDisabled.Value;

            SignalsContext.Users.Update(entity);
            SignalsContext.SaveChanges();

            return Accepted(new UserModel.Read
            {
                Id = entity.Id,
                Username = entity.Username,
                IsAdmin = entity.IsAdmin,
                IsDisabled = entity.IsDisabled
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
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
