using Common.Http;
using Common.Models;
using Auth.Services.Interfaces;
using Common.DTOs;
using System.Net;


namespace Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly IAppHttpClient _client;

        public AuthService(IConfiguration configuration, ILogger<AuthService> logger, IAppHttpClient client)
        {
            _configuration = configuration;
            _logger = logger;
            _client = client;
        }

        public async Task<UserRead?> GetUserByCredentialsAsync(LoginCredentialsModel userCredentials)
        {
            try
            {
                var response = await _client.HttpPostAsync<UserRead,LoginCredentialsModel>(userCredentials);

                LogUserInformation(response.Data);

                return response.statusCode switch
                {
                    HttpStatusCode.OK => response.Data,
                    HttpStatusCode.InternalServerError => HandleInternalServerError(response.message, response.Data),
                    _ => HandleOtherStatusCodes(response.message, response.Data),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user by credentials.");
                return null;
            }
        }

        private void LogUserInformation(UserRead user)
        {
            if (user != null)
            {
                _logger.LogInformation($"User {user.Username} found.");
            }
        }

        private UserRead HandleInternalServerError(string message, UserRead data)
        {
            _logger.LogError(message);
            return data;
        }

        private UserRead HandleOtherStatusCodes(string message, UserRead data)
        {
            _logger.LogWarning(message);
            return data;
        }
    }
}
