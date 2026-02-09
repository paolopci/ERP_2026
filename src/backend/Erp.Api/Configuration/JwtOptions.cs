namespace Erp.Api.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "erp-api";

    public string Audience { get; set; } = "erp-web";

    public string SigningKey { get; set; } = "CHANGE_ME_WITH_A_LONG_RANDOM_SECRET_32+";

    public int ExpirationMinutes { get; set; } = 15;

    public int RefreshTokenDays { get; set; } = 14;
}
