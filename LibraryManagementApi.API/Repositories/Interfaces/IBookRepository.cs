using LibraryManagementApi.API.Models;

namespace LibraryManagementApi.API.Repositories.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<Book?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<Book> CreateAsync(
        Book book,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Book book,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Book book,
        CancellationToken cancellationToken);
}