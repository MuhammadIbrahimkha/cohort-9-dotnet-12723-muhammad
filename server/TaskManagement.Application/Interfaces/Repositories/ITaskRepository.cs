using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories;

/// <summary>
/// Defines data access operations for <see cref="TaskItem"/> entities.
/// </summary>
public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(int id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task AddAsync(TaskItem task);
    void Update(TaskItem task);
    void Delete(TaskItem task);
    Task<bool> SaveChangesAsync();
}