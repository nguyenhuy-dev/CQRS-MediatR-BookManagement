using BookManagement.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Data;

public class BookManagementDbContext : DbContext
{
    public DbSet<Book> Books { get; set; } = default!;
    public DbSet<Author> Authors { get; set; } = default!;
    public BookManagementDbContext()
    {
    }
    public BookManagementDbContext(DbContextOptions<BookManagementDbContext> options) : base(options)
    {
    }
}
