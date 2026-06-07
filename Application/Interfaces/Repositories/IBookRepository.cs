using Application.DTOs.Analitics;
using Application.DTOs.BookDto;
using Application.Results;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IQueryable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Book book);
    Task<PagedResult<GetBookDto>> GetFilteredAsync(GetBookFilterDto filter);

    Task<BookStatisticsDto> GetStatisticsDtoAsync();

}
