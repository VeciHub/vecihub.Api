using Microsoft.AspNetCore.Mvc;
using VeciHub.IAM.Application.Internal.CommandServices;
using VeciHub.IAM.Domain.Commands;

namespace VeciHub.IAM.Interfaces.REST
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserCommandService _userService;

        public AuthenticationController(UserCommandService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SignUpCommand command)
        {
            var token = await _userService.RegisterAsync(command);
            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInCommand command)
        {
            var token = await _userService.LoginAsync(command);
            return Ok(new { Token = token });
        }
    }
}
