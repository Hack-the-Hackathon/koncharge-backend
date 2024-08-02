using Microsoft.AspNetCore.Mvc;

namespace KonChangeAPI.Controllers
{
    [ApiController]
    [Route("testRequest")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public TestController(ILogger<WeatherForecastController> logger)
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
