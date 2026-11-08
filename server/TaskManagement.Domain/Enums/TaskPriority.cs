namespace TaskManagement.Domain.Enums;

/// <summary>
/// Represents how urgent or important a task is.
/// </summary>
public enum TaskPriority
{
    /// <summary>
    /// Low urgency; can be done whenever time allows.
    /// </summary>
    Low = 0,

    /// <summary>
    /// Normal urgency; should be done in a reasonable timeframe.
    /// </summary>
    Medium = 1,

    /// <summary>
    /// High urgency; should be prioritized over other tasks.
    /// </summary>
    High = 2
}