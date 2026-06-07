using Application.DTOs.MemberDto;
using Application.Results;

namespace Application.Interfaces;

public interface IMemberService
{
    Task<PagedResult<GetMemberDto>> GetAllAsync(GetMemberFillterDto dto);
    Task<Result<GetMemberDto?>> GetByIdAsync(int id);
    Task<Result<string>> CreateMemberAsync(CreateMemberDto dto);
    Task<Result<string>> UpdateMemberAsync(int id, UpdateMemberDto dto);
    Task<Result<string>> DeleteMemberAsync(int id);
}
