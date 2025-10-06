using BookManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.MigrationService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        builder.Configuration
            .AddJsonFile("/run/secrets/connection_strings", optional: true);

        var connectionString = builder.Configuration.GetConnectionString("BookManagementDatabase");

        builder.Services.AddDbContext<BookManagementDbContext>(options => options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(typeof(BookManagementDbContext).Assembly.FullName)));
        
        var host = builder.Build();
        host.Run();
    }
}