using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using TaskManagement.API.Controllers;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces.Services;
using Xunit;

namespace TaskManagement.Tests.Controllers;

public class TasksControllerTests
{
    private readonly Mock<ITaskService> _taskServiceMock;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _taskServiceMock = new Mock<ITaskService>();
        _controller = new TasksController(_taskServiceMock.Object);
    }

    [Fact]
    public async Task GetById_TaskDoesNotExist_ReturnsNotFound()
    {
        _taskServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskItemDto?)null);

        var result = await _controller.GetById(999);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_TaskExists_ReturnsOkWithTask()
    {
        var task = new TaskItemDto { Id = 1, Title = "Test", Status = "Pending", Priority = "Medium", CategoryId = 1, AssignedToUserId = 1, CreatedByUserId = 1, CreatedAt = DateTime.UtcNow };
        _taskServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(task);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(task, okResult.Value);
    }

    [Fact]
    public async Task GetAll_AsAdmin_ReturnsOkWithTaskList()
    {
        var tasks = new List<TaskItemDto>
    {
        new() { Id = 1, Title = "T1", Status = "Pending", Priority = "Low", CategoryId = 1, AssignedToUserId = 1, CreatedByUserId = 1, CreatedAt = DateTime.UtcNow }
    };
        _taskServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(tasks);

        var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, "1"),
        new(ClaimTypes.Role, "Admin")
    };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        var result = await _controller.GetAll(null, null, null);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(tasks, okResult.Value);
    }

    [Fact]
    public async Task Update_TaskDoesNotExist_ReturnsNotFound()
    {
        _taskServiceMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateTaskDto>())).ReturnsAsync(false);

        var dto = new UpdateTaskDto { Title = "Updated", Status = "Completed", Priority = "Low", CategoryId = 1, AssignedToUserId = 1 };
        var result = await _controller.Update(999, dto);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}