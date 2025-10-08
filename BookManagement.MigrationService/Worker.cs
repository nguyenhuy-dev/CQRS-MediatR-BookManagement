using BookManagement.Infrastructure.Data;
using BookManagement.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookManagement.MigrationService
{
    /// <summary>
    /// Run under application.
    /// Supporting migration service.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.BackgroundService" />
    public class Worker : BackgroundService
    {
        /// <summary>
        /// The host application lifetime
        /// </summary>
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<Worker> _logger;
        /// <summary>
        /// The service provider
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="hostApplicationLifetime">The host application lifetime.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public Worker(IHostApplicationLifetime hostApplicationLifetime, ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
        /// <remarks>
        /// See <see href="https://docs.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.
        /// </remarks>
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

        /// <summary>
        /// Ensures the database asynchronous.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="stoppingToken">The stopping token.</param>
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

        /// <summary>
        /// Seeds the data asynchronous.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="stoppingToken">The stopping token.</param>
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

        /// <summary>
        /// Runs the migration asynchronous.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="stoppingToken">The stopping token.</param>
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
