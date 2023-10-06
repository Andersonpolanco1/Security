using Auth.Models;
using Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public LoginController(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] LoginRequestModel loginRequest)
        {
            var user = _userService.GetUserByCredentials(loginRequest.Username,loginRequest.Password);
            return user == null ? Ok("Invalid loggin attemp") : Ok(_tokenService.GenerateToken(user));
        }
    }
}
