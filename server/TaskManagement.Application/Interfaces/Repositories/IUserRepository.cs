using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories;

/// <summary>
/// Defines data access operations for <see cref="User"/> entities.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    Task<User?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    Task<IEnumerable<User>> GetAllAsync();

    /// <summary>
    /// Adds a new user.
    /// </summary>
    Task AddAsync(User user);

    /// <summary>
    /// Persists any pending changes to the database.
    /// </summary>
    Task<bool> SaveChangesAsync();
}