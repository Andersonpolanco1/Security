using Microsoft.AspNetCore.Identity;

namespace Auth.Models
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
    }
}
