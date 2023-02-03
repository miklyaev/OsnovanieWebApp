using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using OsnovanieService;
using ILogger = Serilog.ILogger;

namespace OsnovanieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMainService _svc;
        public KafkaController(ILogger logger, IMainService svc)
        {
            _logger = logger;
            _svc = svc;
        }

        [HttpPost]
        [Route("addMessage")]
        public ApiResponse AddUserToKafka(User user)
        {
            try
            {
                var result = _svc.AddUserToKafka(user);
                return new ApiResponse(result.Result, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("readMessage")]
        public ApiResponse ReadFromKafka(string topic)
        {
            try
            {
                var result = _svc.ReadFromKafka(topic);
                return new ApiResponse(result.Result, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }


    }

}
