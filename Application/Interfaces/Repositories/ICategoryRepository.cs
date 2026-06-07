using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<IQueryable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
}
