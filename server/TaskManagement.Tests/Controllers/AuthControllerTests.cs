using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.API.Controllers;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces.Services;
using Xunit;

namespace TaskManagement.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Register_ValidData_ReturnsOkWithAuthResponse()
    {
        var response = new AuthResponseDto { Token = "fake-token", UserId = 1, FullName = "Test", Email = "test@example.com", Role = "User" };
        _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>())).ReturnsAsync(response);

        var request = new RegisterRequestDto { FullName = "Test", Email = "test@example.com", Password = "Test@123" };
        var result = await _controller.Register(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task Register_DuplicateEmail_ReturnsConflict()
    {
        _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
            .ThrowsAsync(new InvalidOperationException("A user with this email already exists."));

        var request = new RegisterRequestDto { FullName = "Test", Email = "test@example.com", Password = "Test@123" };
        var result = await _controller.Register(request);

        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        _authServiceMock.Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>())).ReturnsAsync((AuthResponseDto?)null);

        var request = new LoginRequestDto { Email = "test@example.com", Password = "Wrong@123" };
        var result = await _controller.Login(request);

        Assert.IsType<UnauthorizedObjectResult>(result.Result);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        var response = new AuthResponseDto { Token = "fake-token", UserId = 1, FullName = "Test", Email = "test@example.com", Role = "User" };
        _authServiceMock.Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>())).ReturnsAsync(response);

        var request = new LoginRequestDto { Email = "test@example.com", Password = "Test@123" };
        var result = await _controller.Login(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }
}