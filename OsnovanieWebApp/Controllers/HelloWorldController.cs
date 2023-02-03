using Microsoft.AspNetCore.Mvc;
using OsnovanieService;
using Swashbuckle.AspNetCore.Annotations;
using ILogger = Serilog.ILogger;

namespace OsnovanieWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMainService _svc;

        public HelloWorldController(ILogger logger, IMainService svc)
        {
            _logger = logger;
            _svc = svc;
        }

        [HttpGet(Name = "GetHelloWorld")]
        [SwaggerOperation(Summary = "HelloWorld")]
        public string GetHelloWorld()
        {
            return _svc.GetHelloWorld();
        }
    }
}
