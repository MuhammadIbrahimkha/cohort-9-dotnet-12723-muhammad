using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

/// <summary>
/// Represents an application user who can log in and own or be assigned tasks.
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The user's full display name.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// The user's email address. Used as the login identifier and must be unique.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The hashed password. Never store or transmit plain-text passwords.
    /// </summary>
    public required string PasswordHash { get; set; }

    /// <summary>
    /// The role that determines the user's permissions within the system.
    /// </summary>
    public UserRole Role { get; set; } = UserRole.User;

    /// <summary>
    /// UTC timestamp of when the user account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Tasks created by this user.
    /// </summary>
    public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();

    /// <summary>
    /// Tasks currently assigned to this user.
    /// </summary>
    public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
}