using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Persistense.Repositories;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
    public async Task AddAsync(Member member)
    {
        context.Members.Add(member);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Member member)
    {
        context.Members.Remove(member);
        await context.SaveChangesAsync();
    }

    public async Task<IQueryable<Member>> GetAllAsync()
    {
        return context.Members.AsQueryable();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await context.Members.FindAsync(id);
    }

    public async Task UpdateAsync(Member member)
    {
        context.Members.Update(member);
        await context.SaveChangesAsync();
    }

}
