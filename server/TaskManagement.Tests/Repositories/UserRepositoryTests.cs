using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;
using Xunit;

namespace TaskManagement.Tests.Repositories;

public class UserRepositoryTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetByEmailAsync_ExistingEmail_ReturnsUser()
    {
        await using var context = CreateContext();
        var user = new User { FullName = "Jane Doe", Email = "jane@example.com", PasswordHash = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);
        var result = await repository.GetByEmailAsync("jane@example.com");

        Assert.NotNull(result);
        Assert.Equal("Jane Doe", result!.FullName);
    }

    [Fact]
    public async Task GetByEmailAsync_NonExistentEmail_ReturnsNull()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);

        var result = await repository.GetByEmailAsync("nobody@example.com");

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ThenSaveChanges_PersistsUser()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);

        var user = new User { FullName = "New User", Email = "new@example.com", PasswordHash = "hash" };
        await repository.AddAsync(user);
        await repository.SaveChangesAsync();

        Assert.Equal(1, await context.Users.CountAsync());
    }
}