using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Persistense.Repositories;

public class BorrowRepository(AppDbContext context) : IBorrowRepository
{
    public async Task AddAsync(Borrow borrow)
    {
        context.Borrows.Add(borrow);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Borrow borrow)
    {
        context.Borrows.Remove(borrow);
        await context.SaveChangesAsync();
    }

    public async Task<IQueryable<Borrow>> GetAllAsync()
    {
        return context.Borrows.AsQueryable();
    }

    public async Task<Borrow?> GetByIdAsync(int id)
    {
        return await context.Borrows.FindAsync(id);
    }

    public async Task UpdateAsync(Borrow borrow)
    {
        context.Borrows.Update(borrow);
        await context.SaveChangesAsync();
    }

}
