namespace TaskManagement.Application.DTOs.Auth;

/// <summary>
/// Data required to log in an existing user.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// The user's registered email address.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public required string Password { get; set; }
}