using Microsoft.Extensions.Configuration;
using Moq;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Services;
using Xunit;

namespace TaskManagement.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly IConfiguration _configuration;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();

        var configValues = new Dictionary<string, string?>
        {
            { "Jwt:Key", "TestSecretKeyForUnitTests1234567890123456" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:ExpiryMinutes", "60" }
        };
        _configuration = new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();

        _authService = new AuthService(_userRepoMock.Object, _configuration);
    }

    [Fact]
    public async Task RegisterAsync_WithNewEmail_CreatesUserAndReturnsToken()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

        var request = new RegisterRequestDto { FullName = "Test User", Email = "test@example.com", Password = "Test@123" };
        var result = await _authService.RegisterAsync(request);

        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ThrowsInvalidOperationException()
    {
        var existingUser = new User { Id = 1, FullName = "Existing", Email = "test@example.com", PasswordHash = "hash" };
        _userRepoMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(existingUser);

        var request = new RegisterRequestDto { FullName = "Test User", Email = "test@example.com", Password = "Test@123" };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(request));
    }

    [Fact]
    public async Task LoginAsync_WithCorrectCredentials_ReturnsToken()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Test@123");
        var user = new User { Id = 1, FullName = "Test User", Email = "test@example.com", PasswordHash = hashedPassword, Role = UserRole.User };
        _userRepoMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(user);

        var request = new LoginRequestDto { Email = "test@example.com", Password = "Test@123" };
        var result = await _authService.LoginAsync(request);

        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result!.Token));
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ReturnsNull()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Correct@123");
        var user = new User { Id = 1, FullName = "Test User", Email = "test@example.com", PasswordHash = hashedPassword };
        _userRepoMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(user);

        var request = new LoginRequestDto { Email = "test@example.com", Password = "Wrong@123" };
        var result = await _authService.LoginAsync(request);

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentUser_ReturnsNull()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var request = new LoginRequestDto { Email = "nobody@example.com", Password = "Test@123" };
        var result = await _authService.LoginAsync(request);

        Assert.Null(result);
    }
}