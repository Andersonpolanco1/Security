
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

        public async Task<User> CreateUserAsync(UserCreateModel userCreate)
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

            return user;
        }

        public async Task<User?> GetUserByCredentialsAsync(string username, string password)
        {
            var user = await  _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            if (user is null)
                return user;

            string hashedPassword = BCrypt.HashPassword(password, user.Salt);
            return hashedPassword == user.PasswordHash ? user : null;
        }

        public User GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyPasswordAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return false;
            
            string hashedPassword = BCrypt.HashPassword(password, user.Salt);
            return hashedPassword == user.PasswordHash;
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
