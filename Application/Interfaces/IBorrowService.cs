using Application.DTOs.BorrowDto;
using Application.Results;
namespace Application.Interfaces;

public interface IBorrowService
{
    Task<PagedResult<GetBorrowDto>> GetAllAsync(GetBorrowFillterDto dto);
    Task<Result<GetBorrowDto?>> GetByIdAsync(int id);
    Task<Result<string>> CreateBorrowAsync(CreateBorrowDto dto);
    Task<Result<string>> UpdateBorrowAsync(int id, UpdateBorrowDto dto);
    Task<Result<string>> DeleteBorrowAsync(int id);
}
