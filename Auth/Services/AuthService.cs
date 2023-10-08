
namespace Auth.Services
{
    using Common.Http;
    using Common.Models;
    using Auth.Services.Interfaces;
    using Common.DTOs;


    public class AuthService:IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<User?> GetUserByCredentialsAsync(LoginRequestModel userCredentials)
        {

            var authHeaders = new Dictionary<string, string>() { { "ApiKey", _configuration["UserManagerApiKey"] } };
            var uri = _configuration["Urls:UserManager"];

            var res = await HttpRequest.HttpPostAsync<User,LoginRequestModel>(uri, userCredentials, authHeaders);
            return res.Data;
        }
    }

}
