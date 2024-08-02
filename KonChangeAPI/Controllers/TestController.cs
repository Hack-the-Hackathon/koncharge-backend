using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KonChargeAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("testRequest")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTestrequest")]
        public IEnumerable<string> Get()
        {
            List<string> result = new List<string>();
            result.Add("Hello world");
            return result;
        }
    }
}
