using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class LoginCredentialsModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
