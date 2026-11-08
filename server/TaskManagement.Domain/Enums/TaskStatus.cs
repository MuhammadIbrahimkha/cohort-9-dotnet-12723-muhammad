namespace TaskManagement.Domain.Enums;

/// <summary>
/// Represents the current progress state of a task.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// The task has not been started yet.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// The task is currently being worked on.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The task has been finished.
    /// </summary>
    Completed = 2
}