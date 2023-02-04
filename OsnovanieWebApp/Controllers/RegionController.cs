using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OsnovanieService;
using ILogger = Serilog.ILogger;

namespace OsnovanieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMainService _svc;

        public RegionController(ILogger logger, IMainService svc)
        {
            _logger = logger;
            _svc = svc;
        }
        //[HttpGet]
        //[Route("getRegion")]
        //public ApiResponse GetRegion(int regionId)
        //{
        //    try
        //    {
        //        //var list = _svc.GetRegion(regionId);
        //        return new ApiResponse(list.Result, StatusCodes.Status200OK);
        //    }
        //    catch (Exception exc)
        //    {
        //        throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
        //    }
        //}

        [HttpPost]
        [Route("addRegion")]
        public ApiResponse AddRegion(Region region)
        {
            try
            {
                var result = _svc.AddRegion(region);
                return new ApiResponse(result.Result, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
