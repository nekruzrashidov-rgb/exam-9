using Application.DTOs.Analitics;
using Application.DTOs.BookDto;
using Application.Interfaces.Repositories;
using Application.Results;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistense.Repositories;

public class BookRepository(AppDbContext context) : IBookRepository
{
    public async Task AddAsync(Book book)
    {
        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Book book)
    {
        context.Books.Remove(book);
        await context.SaveChangesAsync();
    }

    public async Task<IQueryable<Book>> GetAllAsync()
    {
        return context.Books.AsQueryable();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await context.Books.FindAsync(id);
    }

    public Task<PagedResult<GetBookDto>> GetFilteredAsync(GetBookFilterDto filter)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Book book)
    {
        context.Books.Update(book);
        await context.SaveChangesAsync();
    }

    public async Task<BookStatisticsDto> GetStatisticsDtoAsync()
    {
        var totalBooks = await context.Books.CountAsync();
        var averagePrice = await context.Books.AverageAsync(b => b.Price);
        var totalBorrows = await context.Borrows.CountAsync();
        
        return new BookStatisticsDto
        {
            TotalBooks = totalBooks,
            AveragePrice = averagePrice,
            TotalBorrows = totalBorrows
        };
    }

}
