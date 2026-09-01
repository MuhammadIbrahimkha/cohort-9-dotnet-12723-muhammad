using TaskManagement.Application.DTOs.Auth;

namespace TaskManagement.Application.Interfaces.Services;

/// <summary>
/// Defines authentication operations such as registration and login.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user and returns an authentication result.
    /// </summary>
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    /// <summary>
    /// Authenticates an existing user and returns an authentication result.
    /// </summary>
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
}