
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

            //using var httpClient = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, _configuration["Urls:UserManager"]);
            //string jsonContent = JsonConvert.SerializeObject(userCredentials);
            //request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            //request.Headers.Authorization = new AuthenticationHeaderValue("ApiKey", _configuration["UserManagerApiKey"]);
            //HttpResponseMessage response = await httpClient.SendAsync(request);
            //return null;

            var res = await HttpRequest.TryGetAsync<User>(userCredentials);
            return res.Data;
        }
    }

}
