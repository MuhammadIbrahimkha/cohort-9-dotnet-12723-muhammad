namespace TaskManagement.Domain.Entities;

/// <summary>
/// Represents a grouping/category that tasks can be organized under (e.g. "Work", "Personal").
/// </summary>
public class Category
{
    /// <summary>
    /// Unique identifier for the category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The display name of the category. Must be unique.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Tasks that belong to this category.
    /// </summary>
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}