using LibraryManagementApi.API.DTOs;
using LibraryManagementApi.API.Models;
using LibraryManagementApi.API.Repositories.Interfaces;
using LibraryManagementApi.API.Services.Interfaces;

namespace LibraryManagementApi.API.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var books = await _repository.GetAllAsync(
            page,
            pageSize,
            cancellationToken);

        return books.Select(MapToResponseDto);
    }

    public async Task<BookResponseDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        return book is null
            ? null
            : MapToResponseDto(book);
    }

    public async Task<BookResponseDto> CreateAsync(
        CreateBookDto dto,
        CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN,
            PublishedYear = dto.PublishedYear,
            AvailableCopies = dto.AvailableCopies
        };

        var createdBook = await _repository.CreateAsync(
            book,
            cancellationToken);

        return MapToResponseDto(createdBook);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateBookDto dto,
        CancellationToken cancellationToken)
    {
        var existingBook = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (existingBook is null)
        {
            return false;
        }

        existingBook.Title = dto.Title;
        existingBook.Author = dto.Author;
        existingBook.ISBN = dto.ISBN;
        existingBook.PublishedYear = dto.PublishedYear;
        existingBook.AvailableCopies = dto.AvailableCopies;

        await _repository.UpdateAsync(
            existingBook,
            cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var existingBook = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (existingBook is null)
        {
            return false;
        }

        await _repository.DeleteAsync(
            existingBook,
            cancellationToken);

        return true;
    }

    private static BookResponseDto MapToResponseDto(Book book)
    {
        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            PublishedYear = book.PublishedYear,
            AvailableCopies = book.AvailableCopies,
            CreatedAt = book.CreatedAt
        };
    }
}