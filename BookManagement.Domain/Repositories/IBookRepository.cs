using BookManagement.Domain.Entities;

namespace BookManagement.Domain.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAsync(CancellationToken cancellationToken = default);

        Task<Book> GetByIdAsync(int bookId, CancellationToken cancellationToken = default);

        void Insert(Book book);
    }
}
