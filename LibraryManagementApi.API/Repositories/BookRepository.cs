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

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);

        await _context.SaveChangesAsync();

        return book;
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Book book)
    {
        _context.Books.Remove(book);

        await _context.SaveChangesAsync();
    }
}