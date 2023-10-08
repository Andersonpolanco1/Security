using Common.DTOs;
using Common.Http;
using Common.Models;

namespace Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> GetUserByCredentialsAsync(LoginRequestModel credentials);
    }
}
