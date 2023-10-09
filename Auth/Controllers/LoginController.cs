using Auth.Models;
using Auth.Services;
using Auth.Services.Interfaces;
using Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;

        public LoginController(ITokenService tokenService, IAuthService authService)
        {
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        public async Task<IActionResult> Post([FromBody] LoginCredentialsModel loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState["Errors"]);

            var user = await _authService.GetUserByCredentialsAsync(loginRequest);
            return user == null ? Ok("Invalid loggin attemp") : Ok(_tokenService.GenerateToken(user));
        }
    }
}
