using LibraryManagementApi.API.DTOs;
using LibraryManagementApi.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementApi.API.Common;

namespace LibraryManagementApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;

    public BooksController(IBookService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var books = await _service.GetAllAsync(
            page,
            pageSize,
            cancellationToken);

        return Ok(
    ApiResponse<IEnumerable<BookResponseDto>>
        .SuccessResponse(
            books,
            "Books retrieved successfully"));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var book = await _service.GetByIdAsync(
            id,
            cancellationToken);

        if (book is null)
        {
            return NotFound();
        }

        return Ok(
    ApiResponse<BookResponseDto>
        .SuccessResponse(
            book,
            "Book retrieved successfully"));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        CreateBookDto dto,
        CancellationToken cancellationToken = default)
    {
        var createdBook = await _service.CreateAsync(
            dto,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdBook.Id },
            ApiResponse<BookResponseDto>
    .SuccessResponse(
        createdBook,
        "Book created successfully"));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        UpdateBookDto dto,
        CancellationToken cancellationToken = default)
    {
        var updated = await _service.UpdateAsync(
            id,
            dto,
            cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var deleted = await _service.DeleteAsync(
            id,
            cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}