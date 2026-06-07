using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IMemberRepository
{
    Task<IQueryable<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task AddAsync(Member member);
    Task UpdateAsync(Member member);
    Task DeleteAsync(Member member);
}
