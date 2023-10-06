using Auth.Models;
using Auth.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserCreateModel userCreate)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var userCreated = _userService.CreateUser(userCreate);
            return Ok(userCreated); 
        }
    }
}
