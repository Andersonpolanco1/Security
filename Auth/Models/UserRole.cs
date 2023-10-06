using Microsoft.AspNetCore.Identity;

namespace Auth.Models
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public List<User> Users { get; set; }
    }
}
