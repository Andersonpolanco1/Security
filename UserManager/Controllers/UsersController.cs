
using Microsoft.AspNetCore.Mvc;
using Common.Interfaces;
using Common.Models;
using Common.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace UserManager.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
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
        public async Task<IActionResult> CreateUser(UserCreateModel userCreate)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var userCreated = await _userService.CreateUserAsync(userCreate);
            return Ok(userCreated); 
        }

        [HttpPost("Credentials")]
        public async Task<IActionResult> FindByCredentials([FromBody] LoginRequestModel param)
        {
            var userCreated = await _userService.GetUserByCredentialsAsync(param.Username,param.Password);
            return Ok(userCreated);
        }

        [HttpPost("VerifyPassword")]
        public async Task<IActionResult> VerifyPasswordAsync([FromBody] LoginRequestModel param)
        {
            var userCreated = await _userService.VerifyPasswordAsync(param.Username, param.Password);
            return Ok(userCreated);
        }
    }
}
