namespace Erp.Contracts.Security;

public sealed record TokenResponse(
    string AccessToken,
    DateTimeOffset ExpiresAtUtc,
    string RefreshToken,
    string TokenType = "Bearer");
