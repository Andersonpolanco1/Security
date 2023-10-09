
namespace UserManager.Services
{
    using BCrypt.Net;
    using System;
    using UserManager.Data;
    using Microsoft.EntityFrameworkCore;
    using Common.Interfaces;
    using Common.Models;
    using Common.DTOs;

    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserRead> CreateUserAsync(UserCreateModel userCreate)
        {
            string salt = BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.HashPassword(userCreate.Password, salt);
            
            var user = new User
            {
                Username = userCreate.Username,
                PasswordHash = hashedPassword,
                Salt = salt,
                Email = userCreate.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.AsUserRead();
        }

        public async Task<UserRead?> GetUserByCredentialsAsync(LoginCredentialsModel credentials)
        {
            var user = await  _context.Users.FirstOrDefaultAsync(u => u.Username == credentials.Username);

            if (user is null)
                return null;

            string hashedPassword = BCrypt.HashPassword(credentials.Password, user.Salt);
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
    }
}
