﻿
using Microsoft.AspNetCore.Mvc;
using Common.Interfaces;
using Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Common.DTOs.User;

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
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CreateUser(UserCreateModel userCreate)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            try
            {
                var userCreated = await _userService.CreateUserAsync(userCreate);
                return Ok(userCreated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserRead>))]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRead))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var users = await _userService.GetUserByIdAsync(id);
            return Ok(users);
        }


        [Authorize(AuthenticationSchemes = "ApiKey")]
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
