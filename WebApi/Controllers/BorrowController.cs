using Application.DTOs.BorrowDto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/borrows")]
public class BorrowController : BaseController
{
    private readonly IBorrowService _borrowService;

    public BorrowController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBorrows([FromQuery] GetBorrowFillterDto filter)
    {
        var result = await _borrowService.GetAllAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBorrowById(int id)
    {
        var result = await _borrowService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBorrow([FromBody] CreateBorrowDto dto)
    {
        var result = await _borrowService.CreateBorrowAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBorrow(int id, [FromBody] UpdateBorrowDto dto)
    {
        var result = await _borrowService.UpdateBorrowAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBorrow(int id)
    {
        var result = await _borrowService.DeleteBorrowAsync(id);
        return Ok(result);
    }
}