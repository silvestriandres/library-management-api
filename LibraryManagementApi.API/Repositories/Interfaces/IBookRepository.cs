using LibraryManagementApi.API.Models;

namespace LibraryManagementApi.API.Repositories.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book?> GetByIdAsync(int id);

    Task<Book> CreateAsync(Book book);

    Task UpdateAsync(Book book);

    Task DeleteAsync(Book book);
}