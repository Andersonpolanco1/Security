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
        public ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options,logger,encoder,clock)
        {
                
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail("Please insert apikey string in header"));
            }

            var keyArray = authorization.FirstOrDefault()?.Split(" ");
            var pairOfKeyValue = 2;
            if (keyArray == null || keyArray.Length != pairOfKeyValue)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid apikey string in header"));
            }

            var configuration = Context.RequestServices.GetRequiredService<IConfiguration>();
            var apikey = configuration["ApiKey"];

            if (!apikey.Equals(keyArray[1]))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid apikey"));
            }

            var identity = new ClaimsIdentity("ApiKey");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

    }
}
