using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories;

/// <summary>
/// Defines data access operations for <see cref="Category"/> entities.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Retrieves a category by its unique identifier.
    /// </summary>
    Task<Category?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    Task<IEnumerable<Category>> GetAllAsync();

    /// <summary>
    /// Adds a new category.
    /// </summary>
    Task AddAsync(Category category);

    /// <summary>
    /// Persists any pending changes to the database.
    /// </summary>
    Task<bool> SaveChangesAsync();
}