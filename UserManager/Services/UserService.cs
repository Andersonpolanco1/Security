
namespace UserManager.Services
{
    using BCrypt.Net;
    using System;
    using UserManager.Data;
    using Microsoft.EntityFrameworkCore;
    using Common.Interfaces;
    using Common.Models;
    using Common.DTOs;
    using Common.Extensions;
    using Common.DTOs.User;

    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserRead> CreateUserAsync(UserCreateModel userCreate)
        {
            if (UserNameOrEmailIsInUse(userCreate))
                throw new Exception("Username or Email in use");

            string salt = BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.HashPassword(userCreate.Password, salt);

            var user = new User
            {
                Username = userCreate.Username.Trim().Capitalize(),
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                Email = userCreate.Email.Trim().ToLower(),
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User added: {user.Id}");
                return user.AsUserRead();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<UserRead?> GetUserByCredentialsAsync(LoginCredentialsModel credentials)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Email.ToLower() == credentials.Email.ToLower());

            if (user is null)
                return null;

            string hashedPassword = BCrypt.HashPassword(credentials.Password, user.PasswordSalt);
            return hashedPassword == user.PasswordHash ? user.AsUserRead() : null;
        }

        public async Task<UserRead?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user is null ? null : user.AsUserRead();
        }

        public async Task<IEnumerable<UserRead>> GetUsersAsync()
        {
            var users = await _context.Users.AsNoTracking()
                .Select(u => new UserRead()
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    IsActive = u.IsActive
                }).ToListAsync();

            return users;
        }

        private bool UserNameOrEmailIsInUse(UserCreateModel user)
        {
            return _context.Users.Any(u => u.Username == user.Username || u.Email == user.Email);
        }
    }
}
