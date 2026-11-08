using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces.Services;

/// <summary>
/// Defines business operations for managing tasks.
/// </summary>
public interface ITaskService
{
    Task<IEnumerable<TaskItemDto>> GetAllAsync();
    Task<TaskItemDto?> GetByIdAsync(int id);
    Task<TaskItemDto> CreateAsync(CreateTaskDto dto, int createdByUserId);
    Task<bool> UpdateAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteAsync(int id);
}