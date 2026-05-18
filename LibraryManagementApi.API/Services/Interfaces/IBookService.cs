using LibraryManagementApi.API.DTOs;

namespace LibraryManagementApi.API.Services.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<BookResponseDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<BookResponseDto> CreateAsync(
        CreateBookDto dto,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        int id,
        UpdateBookDto dto,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken);
}