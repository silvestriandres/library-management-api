using LibraryManagementApi.API.Data;
using LibraryManagementApi.API.Models;
using LibraryManagementApi.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApi.API.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        return await _context.Books
            .AsNoTracking()
            .OrderBy(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Book?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(
                b => b.Id == id,
                cancellationToken);
    }

    public async Task<Book> CreateAsync(
        Book book,
        CancellationToken cancellationToken)
    {
        await _context.Books.AddAsync(
            book,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return book;
    }

    public async Task UpdateAsync(
        Book book,
        CancellationToken cancellationToken)
    {
        _context.Books.Update(book);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Book book,
        CancellationToken cancellationToken)
    {
        _context.Books.Remove(book);

        await _context.SaveChangesAsync(cancellationToken);
    }
}