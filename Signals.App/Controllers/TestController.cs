using Microsoft.AspNetCore.Mvc;
using Signals.App.Database;

namespace Signals.App.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private SignalsContext SignalsContext { get; set; }

        public TestController(SignalsContext context)
        {
            SignalsContext = context;
        }

        [HttpGet]
        public object Get() => new { Tests = SignalsContext.Tests.ToList() };
    }
}
