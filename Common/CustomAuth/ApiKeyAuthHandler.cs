using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Common.CustomAuth
{
    public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthOptions>
    {
        private ILogger<ApiKeyAuthHandler> _logger;

        public ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock) 
            : base(options,loggerFactory,encoder,clock)
        {
            _logger = loggerFactory.CreateLogger<ApiKeyAuthHandler>();

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                var meessage = "Please insert apikey string in header";
                _logger.LogWarning(meessage);
                return Task.FromResult(AuthenticateResult.Fail(meessage));
            }
            var hasBearerHeader = authorization.Any(a => a.Contains("bearer", StringComparison.CurrentCultureIgnoreCase));
            var keyArray = authorization.FirstOrDefault()?.Split(" ");

            var pairOfKeyValue = 2;
            if (keyArray == null || keyArray.Length != pairOfKeyValue)
            {
                return HandleFailAttemp("Invalid apikey string in header", hasBearerHeader);
            }

            if(!"ApiKey".Equals(keyArray[0], StringComparison.OrdinalIgnoreCase))
            {
                return HandleFailAttemp("Urong key name", hasBearerHeader);
            }

            var configuration = Context.RequestServices.GetRequiredService<IConfiguration>();
            var apikey = configuration["ApiKey"];

            if (!apikey.Equals(keyArray[1]))
            {
                return HandleFailAttemp("Invalid ApiKey", hasBearerHeader);
            }

            var identity = new ClaimsIdentity("ApiKey");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        /// <summary>
        /// If has bearer header, not log this fail. The client is trying authenticate by that way.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="hasBearerHeader"></param>
        /// <returns></returns>
        private Task<AuthenticateResult> HandleFailAttemp(string message, bool hasBearerHeader)
        {
            if(!hasBearerHeader)
                _logger.LogWarning(message);

            return Task.FromResult(AuthenticateResult.Fail(message));
        }

    }
}
