using Common.DTOs.User;
using Common.Models;

namespace Auth.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserRead user);
    }
}
