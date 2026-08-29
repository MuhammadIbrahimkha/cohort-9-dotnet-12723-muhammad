using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Services;

/// <summary>
/// Handles business logic for task management.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskItemDto>> GetAllAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return tasks.Select(MapToDto);
    }

    public async Task<TaskItemDto?> GetByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task is null ? null : MapToDto(task);
    }

    public async Task<TaskItemDto> CreateAsync(CreateTaskDto dto, int createdByUserId)
    {
        if (!Enum.TryParse<Domain.Enums.TaskPriority>(dto.Priority, true, out var priority))
        {
            priority = Domain.Enums.TaskPriority.Medium;
        }

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = priority,
            CategoryId = dto.CategoryId,
            DueDate = dto.DueDate,
            AssignedToUserId = dto.AssignedToUserId,
            CreatedByUserId = createdByUserId
        };

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

        var created = await _taskRepository.GetByIdAsync(task.Id);
        return MapToDto(created!);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task is null)
        {
            return false;
        }

        if (!Enum.TryParse<Domain.Enums.TaskStatus>(dto.Status, true, out var status))
        {
            status = Domain.Enums.TaskStatus.Pending;
        }
        if (!Enum.TryParse<Domain.Enums.TaskPriority>(dto.Priority, true, out var priority))
        {
            priority = Domain.Enums.TaskPriority.Medium;
        }

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = status;
        task.Priority = priority;
        task.CategoryId = dto.CategoryId;
        task.DueDate = dto.DueDate;
        task.AssignedToUserId = dto.AssignedToUserId;
        task.UpdatedAt = DateTime.UtcNow;

        _taskRepository.Update(task);
        await _taskRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task is null)
        {
            return false;
        }

        _taskRepository.Delete(task);
        await _taskRepository.SaveChangesAsync();
        return true;
    }

    private static TaskItemDto MapToDto(TaskItem task)
    {
        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority.ToString(),
            CategoryId = task.CategoryId,
            CategoryName = task.Category?.Name,
            DueDate = task.DueDate,
            AssignedToUserId = task.AssignedToUserId,
            AssignedToUserName = task.AssignedToUser?.FullName,
            CreatedByUserId = task.CreatedByUserId,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}