using Moq;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Services;
using Xunit;

namespace TaskManagement.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepoMock;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _taskRepoMock = new Mock<ITaskRepository>();
        _taskService = new TaskService(_taskRepoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidData_CreatesTaskAndReturnsDto()
    {
        _taskRepoMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);
        _taskRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
        _taskRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TaskItem { Id = 1, Title = "Test Task", AssignedToUserId = 1, CreatedByUserId = 1, CategoryId = 1 });

        var dto = new CreateTaskDto { Title = "Test Task", Priority = "High", CategoryId = 1, AssignedToUserId = 1 };
        var result = await _taskService.CreateAsync(dto, createdByUserId: 1);

        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_TaskDoesNotExist_ReturnsNull()
    {
        _taskRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskItem?)null);

        var result = await _taskService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_TaskDoesNotExist_ReturnsFalse()
    {
        _taskRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskItem?)null);

        var dto = new UpdateTaskDto { Title = "Updated", Status = "Completed", Priority = "Low", CategoryId = 1, AssignedToUserId = 1 };
        var result = await _taskService.UpdateAsync(999, dto);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_TaskExists_ReturnsTrue()
    {
        var task = new TaskItem { Id = 1, Title = "Old Title", AssignedToUserId = 1, CreatedByUserId = 1, CategoryId = 1 };
        _taskRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);
        _taskRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

        var dto = new UpdateTaskDto { Title = "New Title", Status = "Completed", Priority = "Low", CategoryId = 1, AssignedToUserId = 1 };
        var result = await _taskService.UpdateAsync(1, dto);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_TaskDoesNotExist_ReturnsFalse()
    {
        _taskRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskItem?)null);

        var result = await _taskService.DeleteAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_TaskExists_ReturnsTrue()
    {
        var task = new TaskItem { Id = 1, Title = "To Delete", AssignedToUserId = 1, CreatedByUserId = 1, CategoryId = 1 };
        _taskRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);
        _taskRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

        var result = await _taskService.DeleteAsync(1);

        Assert.True(result);
    }
}