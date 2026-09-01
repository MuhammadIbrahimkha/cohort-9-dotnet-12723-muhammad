namespace TaskManagement.Application.DTOs.Auth;

/// <summary>
/// Data required to register a new user.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// The new user's full name.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// The new user's email address, used as the login identifier.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The new user's chosen password (plain text at this stage; will be hashed before storage).
    /// </summary>
    public required string Password { get; set; }
}