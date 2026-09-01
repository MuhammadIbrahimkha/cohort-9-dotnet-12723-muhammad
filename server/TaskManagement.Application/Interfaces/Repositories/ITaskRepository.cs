using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories;

/// <summary>
/// Defines data access operations for <see cref="TaskItem"/> entities.
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// Retrieves a task by its unique identifier.
    /// </summary>
    Task<TaskItem?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all tasks, optionally filtered by the assigned user.
    /// </summary>
    Task<IEnumerable<TaskItem>> GetAllAsync();

    /// <summary>
    /// Adds a new task.
    /// </summary>
    Task AddAsync(TaskItem task);

    /// <summary>
    /// Marks an existing task as updated.
    /// </summary>
    void Update(TaskItem task);

    /// <summary>
    /// Removes a task.
    /// </summary>
    void Delete(TaskItem task);

    /// <summary>
    /// Persists any pending changes to the database.
    /// </summary>
    Task<bool> SaveChangesAsync();
}