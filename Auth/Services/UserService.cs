using DocumentFormat.OpenXml.Math;

namespace Auth.Services
{
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using BCrypt.Net;
    using System;
    using Auth.Models;
    using DocumentFormat.OpenXml.Math;
    using Auth.Data;
    using Auth.Services.Interfaces;

    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User CreateUser(UserCreateModel userCreate)
        {
            string salt = BCrypt.GenerateSalt(12); // 12 es el factor de trabajo
            string hashedPassword = BCrypt.HashPassword(userCreate.Password, salt);
            
            var user = new User
            {
                Username = userCreate.Username,
                PasswordHash = hashedPassword,
                Salt = salt,
                Email = userCreate.Email
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User? GetUserByCredentials(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);

            if (user is null)
                return null;

            string hashedPassword = BCrypt.HashPassword(password, user.Salt);

            return hashedPassword == user.PasswordHash ? user : null;
        }

        public User GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public bool VerifyPassword(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);

            if (user != null)
            {
                // Combina la contraseña proporcionada con la sal almacenada en la base de datos
                string hashedPassword = BCrypt.HashPassword(password, user.Salt);

                // Compara el hash resultante con el hash almacenado en la base de datos
                return hashedPassword == user.PasswordHash;
            }

            return false; // Usuario no encontrado
        }
    }

}
