using Microsoft.AspNetCore.Mvc;
using Signals.App.Database;
using Signals.App.Models;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private SignalsContext SignalsContext { get; set; }

        public UsersController(SignalsContext context)
        {
            SignalsContext = context;
        }

        [HttpGet]
        public List<UserModel> GetAll()
        {
            return SignalsContext.Users
                .Select(e => new UserModel
                {
                    Id = e.Id,
                    Username = e.Username,
                    IsAdmin = e.IsAdmin,
                    IsDisabled = e.IsDisabled
                })
                .ToList();
        }

        [HttpGet("{id}")]
        public UserModel Get(Guid id)
        {
            var entity = SignalsContext.Users
                .FirstOrDefault(u => u.Id == id);

            if (entity == null)
                return null;

            return new UserModel
            {
                Id = entity.Id,
                Username = entity.Username,
                IsAdmin = entity.IsAdmin,
                IsDisabled = entity.IsDisabled
            };
        }
    }
}
