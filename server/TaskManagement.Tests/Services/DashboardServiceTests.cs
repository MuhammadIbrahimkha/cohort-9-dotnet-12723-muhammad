using Moq;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Services;
using Xunit;
using TaskStatusEnum = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Tests.Services;

public class DashboardServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepoMock;
    private readonly DashboardService _dashboardService;

    public DashboardServiceTests()
    {
        _taskRepoMock = new Mock<ITaskRepository>();
        _dashboardService = new DashboardService(_taskRepoMock.Object);
    }

    private static List<TaskItem> SampleTasks() => new()
    {
        new TaskItem { Id = 1, Title = "T1", Status = TaskStatusEnum.Pending, AssignedToUserId = 1, CreatedByUserId = 1, CategoryId = 1 },
        new TaskItem { Id = 2, Title = "T2", Status = TaskStatusEnum.InProgress, AssignedToUserId = 1, CreatedByUserId = 1, CategoryId = 1 },
        new TaskItem { Id = 3, Title = "T3", Status = TaskStatusEnum.Completed, AssignedToUserId = 2, CreatedByUserId = 1, CategoryId = 1 },
    };

    [Fact]
    public async Task GetSummaryAsync_AsAdmin_ReturnsCountsForAllTasks()
    {
        _taskRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(SampleTasks());

        var result = await _dashboardService.GetSummaryAsync(userId: 1, isAdmin: true);

        Assert.Equal(1, result.PendingCount);
        Assert.Equal(1, result.InProgressCount);
        Assert.Equal(1, result.CompletedCount);
    }

    [Fact]
    public async Task GetSummaryAsync_AsRegularUser_ReturnsCountsForOwnTasksOnly()
    {
        _taskRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(SampleTasks());

        var result = await _dashboardService.GetSummaryAsync(userId: 1, isAdmin: false);

        Assert.Equal(1, result.PendingCount);
        Assert.Equal(1, result.InProgressCount);
        Assert.Equal(0, result.CompletedCount); // task 3 belongs to user 2
    }
}