using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

/// <summary>
/// Represents a single task that can be created, assigned, tracked, and completed.
/// </summary>
public class TaskItem
{
    /// <summary>
    /// Unique identifier for the task.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Short, descriptive title of the task.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Optional longer description providing more detail about the task.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The current progress state of the task.
    /// </summary>
    public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pending;

    /// <summary>
    /// The urgency/importance level of the task.
    /// </summary>
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    /// <summary>
    /// Foreign key reference to the category this task belongs to.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Navigation property to the category this task belongs to.
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    /// Optional date by which the task should be completed.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Foreign key reference to the user this task is assigned to.
    /// </summary>
    public int AssignedToUserId { get; set; }

    /// <summary>
    /// Navigation property to the user this task is assigned to.
    /// </summary>
    public User? AssignedToUser { get; set; }

    /// <summary>
    /// Foreign key reference to the user who created this task.
    /// </summary>
    public int CreatedByUserId { get; set; }

    /// <summary>
    /// Navigation property to the user who created this task.
    /// </summary>
    public User? CreatedByUser { get; set; }

    /// <summary>
    /// UTC timestamp of when the task was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp of the last update to the task, if any.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}