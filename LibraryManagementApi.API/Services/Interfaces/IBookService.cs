using LibraryManagementApi.API.DTOs;

namespace LibraryManagementApi.API.Services.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllAsync();

    Task<BookResponseDto?> GetByIdAsync(int id);

    Task<BookResponseDto> CreateAsync(CreateBookDto dto);

    Task<bool> UpdateAsync(int id, UpdateBookDto dto);

    Task<bool> DeleteAsync(int id);
}