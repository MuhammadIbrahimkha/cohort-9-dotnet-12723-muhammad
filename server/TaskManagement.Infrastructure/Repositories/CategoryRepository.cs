using Microsoft.EntityFrameworkCore;
using System;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="ICategoryRepository"/>.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
    public CategoryRepository(AppDbContext context) => _context = context;

    public async Task<Category?> GetByIdAsync(int id) => await _context.Categories.FindAsync(id);
    public async Task<IEnumerable<Category>> GetAllAsync() => await _context.Categories.ToListAsync();
    public async Task AddAsync(Category category) => await _context.Categories.AddAsync(category);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}