
using Common.DTOs;

namespace Common.Models
{
    public class User 
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public bool IsActive { get; set; }
        public List<UserRole> Roles { get; set; }

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
