namespace TaskManagement.Domain.Enums;

/// <summary>
/// Defines the set of roles a user can have within the system.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// A standard user who can manage their own assigned tasks.
    /// </summary>
    User = 0,

    /// <summary>
    /// An administrator who can view and manage all users' tasks.
    /// </summary>
    Admin = 1
}