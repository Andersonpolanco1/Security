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

        public LoginController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] LoginRequestModel loginRequest)
        {
            var user = FindUser(loginRequest);
            return user == null ? Ok("Invalid loggin attemp") : Ok(_tokenService.GenerateToken(user));


        }

        private User? FindUser(LoginRequestModel loginRequest)
        {
            if(loginRequest?.Username == "Apolanco")
            {
                return new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Apolanco",
                    IsActive = true,
                    Email = "Apolanco@gmail.com"
                };
            }

            return null;
        }
    }
}
