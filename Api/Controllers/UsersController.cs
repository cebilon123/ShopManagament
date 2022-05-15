using Api.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpPost("Register")]
        public ActionResult Register(RegisterUserCommand command)
        {
            return Ok();
        }
        
        [HttpPost("Login")]
        public ActionResult Login()
        {
            return Ok();
        }
    }
}