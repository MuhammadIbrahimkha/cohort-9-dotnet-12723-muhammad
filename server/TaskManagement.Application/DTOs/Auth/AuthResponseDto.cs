namespace TaskManagement.Application.DTOs.Auth;

/// <summary>
/// Data returned after a successful authentication (register or login).
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// The signed JWT access token.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// The authenticated user's unique identifier.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// The authenticated user's full name.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// The authenticated user's email address.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The authenticated user's role.
    /// </summary>
    public required string Role { get; set; }
}