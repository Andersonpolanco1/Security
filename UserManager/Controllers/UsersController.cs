
using Microsoft.AspNetCore.Mvc;
using Common.Interfaces;
using Common.Models;
using Common.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace UserManager.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRead))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser(UserCreateModel userCreate)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var userCreated = await _userService.CreateUserAsync(userCreate);
            return Ok(userCreated); 
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserRead>))]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }


        [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRead))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost("auth")]
        public async Task<IActionResult> GetByCredentials([FromBody] LoginCredentialsModel credentials)
        {
            var user = await _userService.GetUserByCredentialsAsync(credentials);
            return user is null ? NoContent() : Ok(user);
        }
    }
}
