using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger<LoggingController> _logger;
        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Logger")]
        public IActionResult Get()
        {
            var ex = new ArgumentException("Outer Exception", new ArgumentNullException("Inner Exception"));
            _logger.LogError(ex, "Exception at LoggingController.Get");
            return Ok();
        }
    }
}
