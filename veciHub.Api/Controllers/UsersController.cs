using Microsoft.AspNetCore.Mvc;
using VeciHub.IAM.Application.Internal.QueryServices;
using VeciHub.IAM.Domain.Model;

namespace VeciHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserQueryService _userService;

        public UsersController(UserQueryService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userService.ListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(Guid id)
        {
            var user = await _userService.FindByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
