using Application.DTOs.Auth;
using Application.Results;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<Result<string>> RegisterAsync(RegisterDto dto);
    Task<Result<UserInfoDto>> LoginAsync(LoginDto dto);

}
