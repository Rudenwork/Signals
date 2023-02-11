using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Signals.App.Controllers.Models;
using Signals.App.Database;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SignalsController : ControllerBase
    {
        private SignalsContext SignalsContext { get; }

        public SignalsController(SignalsContext signalsContext)
        {
            SignalsContext = signalsContext;
        }

        [HttpGet]
        public ActionResult<List<SignalModel.Read>> Get([FromQuery] SubsetModel subset, [FromQuery] SignalModel.Read.Filter filter) 
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult<SignalModel.Read> Get(Guid id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<SignalModel.Read>> Post(SignalModel.Create model)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SignalModel>> Delete(Guid id)
        {
            return Ok();
        }
    }
}
