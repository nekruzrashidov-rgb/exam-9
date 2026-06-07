using Application.DTOs.CategoryDto;
using Application.Results;
namespace Application.Interfaces;

public interface ICategoryService
{
    Task<PagedResult<GetCategoryDto>> GetAllAsync(GetCategoryFillterDto dto);
    Task<Result<GetCategoryDto?>> GetByIdAsync(int id);
    Task<Result<string>> CreateCategoryAsync(CreateCategoryDto dto);
    Task<Result<string>> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
    Task<Result<string>> DeleteCategoryAsync(int id);
    
}
