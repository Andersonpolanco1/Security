using System.Text;

namespace UserManager.Models.Settings
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresInMinutes { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public TimeSpan ClockSkew { get; set; }

        /// <summary>
        /// Section route in appsettings files
        /// </summary>
        public static string SettingPath = "JwtSettings";
    }
}
