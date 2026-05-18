using LibraryManagementApi.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApi.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
}