using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using OsnovanieService;
using ILogger = Serilog.ILogger;

namespace OsnovanieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMainService _svc;

        public UserController(ILogger logger, IMainService svc)
        {
            _logger = logger;
            _svc = svc;
        }
        [HttpGet]
        [Route("getUser")]
        public ApiResponse GetUser(int userId)
        {
            try
            {
                var list = _svc.GetUser(userId);
                return new ApiResponse(list.Result, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("getAll")]
        public ApiResponse GetAllUsers()
        {
            try
            {
                var list = _svc.GetAllUsers();
                return new ApiResponse(list.Result, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("addUser")]
        public ApiResponse AddUser(User user)
        {
            try
            {
                var result = _svc.AddUser(user);
                return new ApiResponse(result.Result, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
