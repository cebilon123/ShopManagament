using Api.Commands;
using Api.Result;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        // tutaj przez konstruktor jest wstrzykniety ten service o ktorym napisalem w program.cs
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost("Register")]
        public ActionResult Register(RegisterUserCommand command)
        {
            var result = _userService.RegisterUser(command);
            return result ? Ok() : BadRequest("Podany email jest zajety, badz wprowadzono bledne dane");
        }
        
        // logowanie, zwracamy ActionResult => to jest resultat http z kodem tj. np. Ok 200 
        // https://news.webping.pl/200-301-404-500-i-505-czyli-co-oznaczaja-najpopularniejsze-odpowiedzi-http/
        // <User> oznacza ze zwrocimy uzytkownika w rezultacie typu Result.User
        [HttpPost("Login")]
        public ActionResult<User> Login(LoginUserCommand command)
        {
            var result = _userService.Login(command);
            return result is null ? BadRequest("Podano bledne dane") : Ok(result);
        }
    }
}