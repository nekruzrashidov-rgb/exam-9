namespace Application.DTOs.MemberDto;

public class GetMemberFillterDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Page { get; set; }
    public int PageSize { get; set; }
}
