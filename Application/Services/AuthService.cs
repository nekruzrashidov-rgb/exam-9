using Application.DTOs.Auth;
using Application.DTOs.MemberDto;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Results;
using Domain.Entities;
namespace Application.Services;

public class AuthService(IMemberRepository memberRepository, IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<Result<UserInfoDto>> LoginAsync(LoginDto dto)
    {
        var member = await memberRepository.GetByEmailAsync(dto.Email);
        if (member == null)
        {
            return Result<UserInfoDto>.Fail("Login or password is incorrect", ErrorType.Validation);
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, member.PasswordHash))
        {
            return Result<UserInfoDto>.Fail("Login or password is incorrect", ErrorType.Validation);
        }

        var token = jwtTokenService.GenerateToken(member);

        var userInfo = new UserInfoDto
        {
            User = new GetMemberDto
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                Role = member.Role,
                RegisteredAt = member.RegisteredAt,
            },
            AccessToken = token
        };
        
        return Result<UserInfoDto>.Ok(userInfo);
    }

    public async Task<Result<string>> RegisterAsync(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            return Result<string>.Fail("Fullname is required", ErrorType.Validation);
        }
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var member = new Member
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Role = Domain.Enums.ApplicationRole.User,
            RegisteredAt = DateTime.UtcNow,
            PasswordHash = passwordHash
        };

        await memberRepository.AddAsync(member);

        return Result<string>.Ok("Registration successfull");
    }

}
