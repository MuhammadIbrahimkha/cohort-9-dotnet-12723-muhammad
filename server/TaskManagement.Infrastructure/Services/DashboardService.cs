using TaskManagement.Application.DTOs.Dashboard;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly ITaskRepository _taskRepository;

    public DashboardService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<DashboardDto> GetSummaryAsync(int userId, bool isAdmin)
    {
        var tasks = await _taskRepository.GetAllAsync();

        if (!isAdmin)
            tasks = tasks.Where(t => t.AssignedToUserId == userId);

        return new DashboardDto
        {
            PendingCount = tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Pending),
            InProgressCount = tasks.Count(t => t.Status == Domain.Enums.TaskStatus.InProgress),
            CompletedCount = tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Completed)
        };
    }
}