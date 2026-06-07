using Application.DTOs.Analitics;
using Application.DTOs.BookDto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/books")]
public class BookController : BaseController
{
    private readonly IBookService _bookService;
    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] GetBookFillterDto filter)
    {
        var result = await _bookService.GetAllAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var result = await _bookService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookDto dto)
    {
        var result = await _bookService.CreateBookAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto dto)
    {
        var result = await _bookService.UpdateBookAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var result = await _bookService.DeleteBookAsync(id);
        return Ok(result);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilteredBooks([FromQuery] GetBookFilterDto filter)
    {
        var result = await _bookService.GetFilteredAsync(filter);
        return Ok(result);
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var result = await _bookService.GetStatisticsAsync();
        return Ok(result);
    }
}
