using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BookManagement.Infrastructure.Data;

public class BookManagementDbContextFactory : IDesignTimeDbContextFactory<BookManagementDbContext>
{
    public BookManagementDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BookManagement"))
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("BookManagementDatabase");

        var optionsBuilder = new DbContextOptionsBuilder<BookManagementDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new BookManagementDbContext(optionsBuilder.Options);
    }
}