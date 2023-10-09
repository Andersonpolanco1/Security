using Auth.Models;
using Auth.Models.Settings;
using Auth.Services.Interfaces;
using Common.DTOs;
using Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Services
{
    public class TokenService: ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(UserRead user)
        {
            var jwtKeyArray = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            var securityKey = new SymmetricSecurityKey(jwtKeyArray);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.Sid,user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,user.Username),
                },
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
