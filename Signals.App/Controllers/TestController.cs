using Microsoft.AspNetCore.Mvc;

namespace Signals.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public object Get() => new { Test = "test" };
    }
}
