using Erp.Application.Abstractions.Security;
using Erp.Application.Common.Security;
using Erp.Application.Security.Auth.Commands.Login;
using Erp.Application.Security.Auth.Commands.RefreshToken;
using Erp.Application.Security.Auth.Commands.RevokeRefreshToken;
using FluentAssertions;
using NSubstitute;

namespace Erp.Application.Tests.Security;

public sealed class AuthCommandsTests
{
    private readonly IIdentityService _identityService;
    private readonly ITokenIssuer _tokenIssuer;

    public AuthCommandsTests()
    {
        _identityService = Substitute.For<IIdentityService>();
        _tokenIssuer = Substitute.For<ITokenIssuer>();
    }

    [Fact]
    public async Task Login_ConCredenzialiValide_RestituisceToken()
    {
        // Arrange
        var user = new AuthenticatedUser(
            Guid.NewGuid(),
            "admin",
            Guid.NewGuid(),
            ["Admin"],
            ["administration.full"]);
        var expected = new Erp.Contracts.Security.TokenResponse("access", DateTimeOffset.UtcNow.AddMinutes(15), "refresh");
        _identityService.ValidateCredentialsAsync("admin", "Password123!", Arg.Any<CancellationToken>())
            .Returns(user);
        _tokenIssuer.IssueTokensAsync(user, Arg.Any<CancellationToken>())
            .Returns(expected);
        var sut = new LoginCommandHandler(_identityService, _tokenIssuer);

        // Act
        var result = await sut.Handle(new LoginCommand("admin", "Password123!"), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Login_ConCredenzialiErrate_RestituisceNull()
    {
        // Arrange
        _identityService.ValidateCredentialsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((AuthenticatedUser?)null);
        var sut = new LoginCommandHandler(_identityService, _tokenIssuer);

        // Act
        var result = await sut.Handle(new LoginCommand("admin", "bad"), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Refresh_ConTokenVuoto_LanciaErroreValidazione()
    {
        // Arrange
        var validator = new RefreshTokenCommandValidator();

        // Act
        var result = await validator.ValidateAsync(new RefreshTokenCommand(string.Empty));

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Revoke_ConTokenValido_RestituisceTrue()
    {
        // Arrange
        _tokenIssuer.RevokeRefreshTokenAsync("refresh-token", Arg.Any<Guid?>(), Arg.Any<CancellationToken>())
            .Returns(true);
        var sut = new RevokeRefreshTokenCommandHandler(_tokenIssuer);

        // Act
        var result = await sut.Handle(new RevokeRefreshTokenCommand("refresh-token", Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}
