namespace Shared.Common.Configuration
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int AccessTokenExpiryInMinutes { get; set; } = 15;
        public int RefreshTokenExpiryInDays { get; set; } = 7;
    }
}