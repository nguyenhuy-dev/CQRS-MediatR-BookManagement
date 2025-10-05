using BookManagement.Domain.Repositories;
using BookManagement.Infrastructure.Data;

namespace BookManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookManagementDbContext _context;

        public UnitOfWork(BookManagementDbContext context) => _context = context;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);
    }
}
