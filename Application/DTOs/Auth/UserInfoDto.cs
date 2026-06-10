using Application.DTOs.MemberDto;

namespace Application.DTOs.Auth;

public class UserInfoDto
{
    public GetMemberDto User { get; set; }
    public string AccessToken { get; set; }
}
