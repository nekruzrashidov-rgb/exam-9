using Application.DTOs.Analitics;
using Application.DTOs.BookDto;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class BookService(IBookRepository bookRepository, ILogger<BookService> logger) : IBookService
{
    public async Task<Result<string>> CreateBookAsync(CreateBookDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return Result<string>.Fail("Title is required");

            if (dto.Title.Length > 100)
                return Result<string>.Fail("Title must be less than 100 characters");

            if (string.IsNullOrWhiteSpace(dto.Author))
                return Result<string>.Fail("Author is required");

            if (dto.Author.Length > 100)
                return Result<string>.Fail("Author must be less than 100 characters");

            if (dto.Price < 0)
                return Result<string>.Fail("Price must be greater than or equal to 0");

            if (dto.PublishedYear <= 2020)
                return Result<string>.Fail("PublishedYear must be greater than or equal to 2020");

            if (dto.CategoryId <= 0)
                return Result<string>.Fail("CategoryId must be greater than 0");

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Price = dto.Price,
                PublishedYear = dto.PublishedYear,
                CategoryId = dto.CategoryId
            };

            await bookRepository.AddAsync(book);
            return Result<string>.Ok("Book created successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating book. DTO: {@Dto}", dto); // ← Добавили детали
            return Result<string>.Fail($"Error: {ex.Message}");
        }
    }

    public async Task<Result<string>> DeleteBookAsync(int id)
    {
        try
        {
            var existingBook = await bookRepository.GetByIdAsync(id);
            if (existingBook == null)
                return Result<string>.Fail("Book not found");

            await bookRepository.DeleteAsync(existingBook);
            return Result<string>.Ok("Book deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting book");
            return Result<string>.Fail("An error occurred while deleting the book");
        }
    }

    public async Task<PagedResult<GetBookDto>> GetAllAsync(GetBookFillterDto dto)  // ← Исправил опечатку
    {
        var query = await bookRepository.GetAllAsync();

        var totalCount = await query.CountAsync();

        var books = await query
            .Skip((dto.Page - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .Select(b => new GetBookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                PublishedYear = b.PublishedYear,
                CategoryId = b.CategoryId
            })
            .ToListAsync();

        return PagedResult<GetBookDto>.Ok(books, totalCount, dto.Page, dto.PageSize);
    }

    public async Task<Result<GetBookDto?>> GetByIdAsync(int id)
    {
        var book = await bookRepository.GetByIdAsync(id);

        if (book == null)
            return null!;

        var dto = new GetBookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            PublishedYear = book.PublishedYear,
            CategoryId = book.CategoryId,
        };

        return Result<GetBookDto?>.Ok(dto);
    }


    public async Task<Result<string>> UpdateBookAsync(int id, UpdateBookDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return Result<string>.Fail("Title is required");

            if (dto.Title.Length > 100)
                return Result<string>.Fail("Title must be less than 100 characters");

            if (string.IsNullOrWhiteSpace(dto.Author))
                return Result<string>.Fail("Author is required");

            if (dto.Author.Length > 100)
                return Result<string>.Fail("Author must be less than 100 characters");

            if (dto.Price < 0)
                return Result<string>.Fail("Price must be greater than or equal to 0");

            if (dto.PublishedYear < 2020)
                return Result<string>.Fail("PublishedYear must be greater than or equal to 2020");

            if (dto.CategoryId <= 0)
                return Result<string>.Fail("CategoryId must be greater than 0");

            var existingBook = await bookRepository.GetByIdAsync(id);
            if (existingBook == null)
                return Result<string>.Fail("Book not found");

            existingBook.Title = dto.Title;
            existingBook.Author = dto.Author;
            existingBook.Price = dto.Price;
            existingBook.PublishedYear = dto.PublishedYear;
            existingBook.CategoryId = dto.CategoryId;

            await bookRepository.UpdateAsync(existingBook);
            return Result<string>.Ok("Book updated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating book");
            return Result<string>.Fail("An error occurred while updating the book");
        }
    }

    public async Task<PagedResult<GetBookDto>> GetFilteredAsync(GetBookFilterDto filter)
    {
        var query = await bookRepository.GetAllAsync();

        if (filter.CategoryId.HasValue)
            query = query.Where(b => b.CategoryId == filter.CategoryId.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(b => b.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(b => b.Price <= filter.MaxPrice.Value);


        var totalCount = await query.CountAsync();

        var books = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(b => new GetBookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                PublishedYear = b.PublishedYear,
                CategoryId = b.CategoryId
            })
            .ToListAsync();

        return PagedResult<GetBookDto>.Ok(books, totalCount, filter.Page, filter.PageSize);
    }

    public async Task<Result<BookStatisticsDto>> GetStatisticsAsync()
    {

        try
        {
            var query = await bookRepository.GetAllAsync();

            var totalBooks = await query.CountAsync();
            var averagePrice = await query.AverageAsync(b => b.Price);
            var minPrice = await query.MinAsync(b => b.Price);
            var maxPrice = await query.MaxAsync(b => b.Price);
        

            var stats = new BookStatisticsDto
            {
                TotalBooks = totalBooks,
                AveragePrice = Math.Round(averagePrice, 2),
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            return Result<BookStatisticsDto>.Ok(stats);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting book statistics");
            return Result<BookStatisticsDto>.Fail("Error retrieving statistics");
        }
    }
}
