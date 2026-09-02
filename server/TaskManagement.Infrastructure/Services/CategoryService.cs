using TaskManagement.Application.DTOs.Categories;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name });
    }

    public async Task<CategoryDto> CreateAsync(string name)
    {
        var category = new Category { Name = name };
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();
        return new CategoryDto { Id = category.Id, Name = category.Name };
    }
}