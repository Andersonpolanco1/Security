using Common.DTOs;
using Common.DTOs.User;
using Common.Http;
using Common.Models;

namespace Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserRead?> GetUserByCredentialsAsync(LoginCredentialsModel credentials);
    }
}
