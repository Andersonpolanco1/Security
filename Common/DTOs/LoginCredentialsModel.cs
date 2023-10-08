using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class LoginCredentialsModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
