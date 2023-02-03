using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using OsnovanieService;
using OsnovanieService.Model;
using ILogger = Serilog.ILogger;

namespace OsnovanieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMainService _svc;

        public RequestController(ILogger logger, IMainService svc)
        {
            _logger = logger;
            _svc = svc;
        }

        [HttpPost]
        [Route("view")]
        public ApiResponse View(Auth user)
        {
            try
            {
                var info = _svc.ViewRequest();
                return new ApiResponse(info, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("list")]
        public async Task<ApiResponse> GetList()
        {
            try
            {
                var list = _svc.ListRequest();
                return new ApiResponse(list, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
