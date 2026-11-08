using TaskManagement.Application.DTOs.Dashboard;

namespace TaskManagement.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetSummaryAsync(int userId, bool isAdmin);
}