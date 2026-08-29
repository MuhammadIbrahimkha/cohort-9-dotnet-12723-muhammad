using Microsoft.EntityFrameworkCore;
using System;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    public TaskRepository(AppDbContext context) => _context = context;

    public async Task<TaskItem?> GetByIdAsync(int id) =>
        await _context.Tasks.Include(t => t.Category).Include(t => t.AssignedToUser).Include(t => t.CreatedByUser)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<TaskItem>> GetAllAsync() =>
        await _context.Tasks.Include(t => t.Category).Include(t => t.AssignedToUser).Include(t => t.CreatedByUser).ToListAsync();

    public async Task AddAsync(TaskItem task) => await _context.Tasks.AddAsync(task);
    public void Update(TaskItem task) => _context.Tasks.Update(task);
    public void Delete(TaskItem task) => _context.Tasks.Remove(task);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}