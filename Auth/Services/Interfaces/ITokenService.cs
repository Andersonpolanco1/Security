using Auth.Models;

namespace Auth.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
