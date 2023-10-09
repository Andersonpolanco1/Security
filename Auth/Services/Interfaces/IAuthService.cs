using Common.DTOs;
using Common.Http;
using Common.Models;

namespace Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserRead?> GetUserByCredentialsAsync(LoginCredentialsModel credentials);
    }
}
