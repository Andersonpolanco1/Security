using Common.DTOs;
using Common.DTOs.User;

namespace Common.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserRead>> GetUsersAsync();    
        Task<UserRead> CreateUserAsync(UserCreateModel userCreate);
        Task<UserRead?> GetUserByCredentialsAsync(LoginCredentialsModel userCredentials);
        Task<UserRead?> GetUserByIdAsync(Guid id);
    }
}
