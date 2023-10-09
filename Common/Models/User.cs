
using Common.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Common.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    public class User 
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsActive { get; set; }
        public List<UserRole> Roles { get; set; }
        public DateTime CreatedAt { get; set; }


        /// <summary>
        /// Method that convert User entity to an entity to expose data outside
        /// </summary>
        /// <returns>UserRead class, User representation outside</returns>
        public UserRead AsUserRead()
        {
            return new UserRead
            {
                Id = Id,
                Username = Username,
                Email = Email,
                IsActive = IsActive
            };
        }
    }
}
