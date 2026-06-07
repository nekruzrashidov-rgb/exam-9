using Application.DTOs.CategoryDto;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger) : ICategoryService
{
    public async Task<Result<string>> CreateCategoryAsync(CreateCategoryDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result<string>.Fail("Name is required");

            if (dto.Name.Length > 100)
                return Result<string>.Fail("Name must be less than 100 characters");

            if (!string.IsNullOrWhiteSpace(dto.Description) && dto.Description.Length > 500)
                return Result<string>.Fail("Description must be less than 500 characters");

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await categoryRepository.AddAsync(category);
            return Result<string>.Ok("Category created successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<string>.Fail("An error occurred while creating the category");
        }
    }

    public async Task<Result<string>> DeleteCategoryAsync(int id)
    {
        try
        {
            var existingCategory = await categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
                return Result<string>.Fail("Category not found");

            await categoryRepository.DeleteAsync(existingCategory);
            return Result<string>.Ok("Category deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<string>.Fail("An error occurred while deleting the category");
        }
    }

    public async Task<PagedResult<GetCategoryDto>> GetAllAsync(GetCategoryFillterDto dto)
    {
        var query = await categoryRepository.GetAllAsync();

        var totalCount = await query.CountAsync();

        var categories = await query
            .Skip((dto.Page - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .Select(c => new GetCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .ToListAsync();

        return PagedResult<GetCategoryDto>.Ok(categories, totalCount, dto.Page, dto.PageSize);
    }

    public async Task<Result<GetCategoryDto?>> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);

        if (category == null)
            return Result<GetCategoryDto?>.Fail("Category not found");

        var dto = new GetCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };

        return Result<GetCategoryDto?>.Ok(dto);
    }

    public async Task<Result<string>> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result<string>.Fail("Name is required");

            if (dto.Name.Length > 100)
                return Result<string>.Fail("Name must be less than 100 characters");

            if (!string.IsNullOrWhiteSpace(dto.Description) && dto.Description.Length > 500)
                return Result<string>.Fail("Description must be less than 500 characters");

            var existingCategory = await categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
                return Result<string>.Fail("Category not found");

            existingCategory.Name = dto.Name;
            existingCategory.Description = dto.Description;

            await categoryRepository.UpdateAsync(existingCategory);
            return Result<string>.Ok("Category updated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<string>.Fail("An error occurred while updating the category");
        }
    }
}