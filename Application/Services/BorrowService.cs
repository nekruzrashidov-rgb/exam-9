using Application.DTOs.BorrowDto;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class BorrowService(IBorrowRepository borrowRepository, ILogger<BorrowService> logger) : IBorrowService
{
    public async Task<Result<string>> CreateBorrowAsync(CreateBorrowDto dto)
    {
        try
        {
            if (dto.BookId <= 0)
                return Result<string>.Fail("BookId is required");

            if (dto.MemberId <= 0)
                return Result<string>.Fail("MemberId is required");

            if (dto.BorrowDate == default)
                return Result<string>.Fail("BorrowDate is required");

            var borrow = new Borrow
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                BorrowDate = dto.BorrowDate,
                ReturnDate = dto.ReturnDate
            };

            await borrowRepository.AddAsync(borrow);
            return Result<string>.Ok("Borrow created successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<string>.Fail("An error occurred while creating the borrow");
        }
    }

    public async Task<Result<string>> DeleteBorrowAsync(int id)
    {
        try
        {
            var existingBorrow = await borrowRepository.GetByIdAsync(id);
            if (existingBorrow == null)
                return Result<string>.Fail("Borrow not found");

            await borrowRepository.DeleteAsync(existingBorrow);
            return Result<string>.Ok("Borrow deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<string>.Fail("An error occurred while deleting the borrow");
        }
    }

    public async Task<PagedResult<GetBorrowDto>> GetAllAsync(GetBorrowFillterDto dto)
    {
        var query = await borrowRepository.GetAllAsync();

        var totalCount = await query.CountAsync();

        var borrows = await query
            .Skip((dto.Page - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .Select(b => new GetBorrowDto
            {
                Id = b.Id,
                BorrowDate = b.BorrowDate,
                ReturnDate = b.ReturnDate,
                BookId = b.BookId,
                MemberId = b.MemberId
            })
            .ToListAsync();

        return PagedResult<GetBorrowDto>.Ok(borrows, totalCount, dto.Page, dto.PageSize);
    }

    public async Task<Result<GetBorrowDto?>> GetByIdAsync(int id)
    {
        var borrow = await borrowRepository.GetByIdAsync(id);

        if (borrow == null)
            return Result<GetBorrowDto?>.Fail("Borrow not found");

        var dto = new GetBorrowDto
        {
            Id = borrow.Id,
            BorrowDate = borrow.BorrowDate,
            ReturnDate = borrow.ReturnDate,
            BookId = borrow.BookId,
            MemberId = borrow.MemberId
        };

        return Result<GetBorrowDto?>.Ok(dto);
    }

    public async Task<Result<string>> UpdateBorrowAsync(int id, UpdateBorrowDto dto)
    {
        try
        {
            if (dto.BookId <= 0)
                return Result<string>.Fail("BookId is required");

            if (dto.MemberId <= 0)
                return Result<string>.Fail("MemberId is required");

            var existingBorrow = await borrowRepository.GetByIdAsync(id);
            if (existingBorrow == null)
                return Result<string>.Fail("Borrow not found");

            existingBorrow.BookId = dto.BookId;
            existingBorrow.MemberId = dto.MemberId;
            existingBorrow.BorrowDate = dto.BorrowDate;
            existingBorrow.ReturnDate = dto.ReturnDate;

            await borrowRepository.UpdateAsync(existingBorrow);
            return Result<string>.Ok("Borrow updated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<string>.Fail("An error occurred while updating the borrow");
        }
    }
}