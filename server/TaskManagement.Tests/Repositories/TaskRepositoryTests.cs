using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;
using Xunit;

namespace TaskManagement.Tests.Repositories;

public class TaskRepositoryTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ThenSaveChanges_PersistsTask()
    {
        await using var context = CreateContext();
        var repository = new TaskRepository(context);

        var task = new TaskItem { Title = "Test Task", AssignedToUserId = 1, CreatedByUserId = 1, CategoryId = 1 };
        await repository.AddAsync(task);
        await repository.SaveChangesAsync();

        Assert.Equal(1, await context.Tasks.CountAsync());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingTask_ReturnsTask()
    {
        await using var context = CreateContext();

        var category = new Category { Name = "Test Category" };
        var user = new User { FullName = "Test User", Email = "test@example.com", PasswordHash = "hash" };
        context.Categories.Add(category);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var task = new TaskItem
        {
            Title = "Findable Task",
            AssignedToUserId = user.Id,
            CreatedByUserId = user.Id,
            CategoryId = category.Id
        };
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var repository = new TaskRepository(context);
        var result = await repository.GetByIdAsync(task.Id);

        Assert.NotNull(result);
        Assert.Equal("Findable Task", result!.Title);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentTask_ReturnsNull()
    {
        await using var context = CreateContext();
        var repository = new TaskRepository(context);

        var result = await repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ExistingTask_RemovesFromDatabase()
    {
        await using var context = CreateContext();
        var task = new TaskItem { Title = "To Delete", AssignedToUserId = 1, CreatedByUserId = 1, CategoryId = 1 };
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var repository = new TaskRepository(context);
        repository.Delete(task);
        await repository.SaveChangesAsync();

        Assert.Equal(0, await context.Tasks.CountAsync());
    }
}