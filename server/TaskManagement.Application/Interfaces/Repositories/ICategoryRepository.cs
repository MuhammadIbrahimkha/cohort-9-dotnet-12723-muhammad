using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories;

/// <summary>
/// Defines data access operations for <see cref="Category"/> entities.
/// </summary>
public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task AddAsync(Category category);
    Task<bool> SaveChangesAsync();
}