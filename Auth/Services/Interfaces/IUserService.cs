using Auth.Models;

namespace Auth.Services.Interfaces
{
    public interface IUserService
    {
        User CreateUser(UserCreateModel userCreate);
        bool VerifyPassword(string username, string password);
        User? GetUserByCredentials(string username, string password);
        User GetUserById(string id);
    }
}
