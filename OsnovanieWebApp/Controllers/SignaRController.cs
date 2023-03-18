﻿using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using OsnovanieService;

namespace OsnovanieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignaRController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMainService _svc;
        public SignaRController(ILogger logger, IMainService svc)
        {
            _logger = logger;
            _svc = svc;
        }

        [HttpPost]
        [Route("addSignal")]
        public ApiResponse AddSignal(Signal signal)
        {
            try
            {
                var result = _svc.AddSignalToKafka(signal);
                return new ApiResponse(result.Result, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
