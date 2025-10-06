using BookManagement.Infrastructure.Data;
using BookManagement.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookManagement.MigrationService
{
    public class Worker : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(IHostApplicationLifetime hostApplicationLifetime, ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Migrating the database...");

                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BookManagementDbContext>();

                _logger.LogInformation("Ensuring the database exists and is up to date...");
                await EnsureDatabaseAsync(dbContext, stoppingToken);

                _logger.LogInformation("Running migration...");
                await RunMigrationAsync(dbContext, stoppingToken);
                _logger.LogInformation("Database migration completed successfully.");

                await SeedDataAsync(dbContext, stoppingToken);
                _logger.LogInformation("Data seeded.");

                //return; // use for be integrated with Web API
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }

            _hostApplicationLifetime.StopApplication(); // using when deploy dependence in Docker
        }

        private async Task EnsureDatabaseAsync(BookManagementDbContext dbContext, CancellationToken stoppingToken)
        {
            var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                if (!await dbCreator.ExistsAsync(stoppingToken))
                {
                    await dbCreator.CreateAsync(stoppingToken);
                }
            });
        }

        private async Task SeedDataAsync(BookManagementDbContext dbContext, CancellationToken stoppingToken)
        {
            Author author = new()
            {
                AuthorName = "J. K. Rowling"
            };

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync(stoppingToken);
                await dbContext.Authors.AddAsync(author, stoppingToken);
                await dbContext.SaveChangesAsync(stoppingToken);
                await transaction.CommitAsync(stoppingToken);
            });
        }

        private async Task RunMigrationAsync(BookManagementDbContext dbContext, CancellationToken stoppingToken)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await dbContext.Database.MigrateAsync(stoppingToken);
            });
        }
    }
}
