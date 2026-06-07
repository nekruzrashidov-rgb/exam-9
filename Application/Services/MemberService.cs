using Application.DTOs.MemberDto;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class MemberService(IMemberRepository memberRepository, ILogger<MemberService> logger) : IMemberService
{
    public async Task<Result<string>> CreateMemberAsync(CreateMemberDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                return Result<string>.Fail("FullName is required");

            if (dto.FullName.Length > 150)
                return Result<string>.Fail("FullName must be less than 150 characters");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return Result<string>.Fail("Email is required");

            if (dto.Email.Length > 150)
                return Result<string>.Fail("Email must be less than 150 characters");

            // Простая проверка email (можно усилить позже)
            if (!dto.Email.Contains("@"))
                return Result<string>.Fail("Invalid email format");

            if (dto.RegisteredAt == default)
                dto.RegisteredAt = DateTime.UtcNow;

            var member = new Member
            {
                FullName = dto.FullName,
                Email = dto.Email,
                RegisteredAt = dto.RegisteredAt
            };

            await memberRepository.AddAsync(member);
            return Result<string>.Ok("Member created successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating member");
            return Result<string>.Fail("An error occurred while creating the member");
        }
    }

    public async Task<Result<string>> DeleteMemberAsync(int id)
    {
        try
        {
            var existingMember = await memberRepository.GetByIdAsync(id);
            if (existingMember == null)
                return Result<string>.Fail("Member not found");

            await memberRepository.DeleteAsync(existingMember);
            return Result<string>.Ok("Member deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting member");
            return Result<string>.Fail("An error occurred while deleting the member");
        }
    }

    public async Task<PagedResult<GetMemberDto>> GetAllAsync(GetMemberFillterDto dto)
    {
        var query = await memberRepository.GetAllAsync();

        var totalCount = await query.CountAsync();

        var members = await query
            .Skip((dto.Page - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .Select(m => new GetMemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                RegisteredAt = m.RegisteredAt
            })
            .ToListAsync();

        return PagedResult<GetMemberDto>.Ok(members, totalCount, dto.Page, dto.PageSize);
    }

    public async Task<Result<GetMemberDto?>> GetByIdAsync(int id)
    {
        var member = await memberRepository.GetByIdAsync(id);

        if (member == null)
            return Result<GetMemberDto?>.Fail("Member not found");

        var dto = new GetMemberDto
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            RegisteredAt = member.RegisteredAt
        };

        return Result<GetMemberDto?>.Ok(dto);
    }

    public async Task<Result<string>> UpdateMemberAsync(int id, UpdateMemberDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                return Result<string>.Fail("FullName is required");

            if (dto.FullName.Length > 150)
                return Result<string>.Fail("FullName must be less than 150 characters");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return Result<string>.Fail("Email is required");

            if (dto.Email.Length > 150)
                return Result<string>.Fail("Email must be less than 150 characters");

            if (!dto.Email.Contains("@"))
                return Result<string>.Fail("Invalid email format");

            var existingMember = await memberRepository.GetByIdAsync(id);
            if (existingMember == null)
                return Result<string>.Fail("Member not found");

            existingMember.FullName = dto.FullName;
            existingMember.Email = dto.Email;
            existingMember.RegisteredAt = dto.RegisteredAt;

            await memberRepository.UpdateAsync(existingMember);
            return Result<string>.Ok("Member updated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating member");
            return Result<string>.Fail("An error occurred while updating the member");
        }
    }
}