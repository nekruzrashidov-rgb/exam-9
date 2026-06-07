using Application.DTOs.Analitics;
using Application.DTOs.BookDto;
using Application.Results;
namespace Application.Interfaces;

public interface IBookService
{
    Task<PagedResult<GetBookDto>> GetAllAsync(GetBookFillterDto dto);
    Task<Result<GetBookDto?>> GetByIdAsync(int id);
    Task<Result<string>> CreateBookAsync(CreateBookDto dto);
    Task<Result<string>> UpdateBookAsync(int id, UpdateBookDto dto);
    Task<Result<string>> DeleteBookAsync(int id);

    Task<PagedResult<GetBookDto>> GetFilteredAsync(GetBookFilterDto filter);

    Task<Result<BookStatisticsDto>> GetStatisticsAsync();
}
