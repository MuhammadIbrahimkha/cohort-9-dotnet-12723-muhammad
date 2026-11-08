namespace TaskManagement.Application.DTOs.Dashboard;

public class DashboardDto
{
    public int PendingCount { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
}