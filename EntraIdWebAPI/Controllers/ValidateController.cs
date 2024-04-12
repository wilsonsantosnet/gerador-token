using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntraIdWebAppSignin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    //[Authorize(Roles ="admin")]
    //[RequiredScope("access.all")]
    public class ValidateController : ControllerBase
    {
       
        private readonly ILogger<ValidateController> _logger;
        private readonly IConfiguration _configuration;

        public ValidateController(ILogger<ValidateController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
        }

        [HttpGet]
        public IActionResult Get()
        {

           return Ok();
            
        }

      

    }
}
