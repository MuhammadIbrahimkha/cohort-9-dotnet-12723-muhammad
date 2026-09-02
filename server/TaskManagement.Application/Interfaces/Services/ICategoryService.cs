using TaskManagement.Application.DTOs.Categories;

namespace TaskManagement.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto> CreateAsync(string name);
}