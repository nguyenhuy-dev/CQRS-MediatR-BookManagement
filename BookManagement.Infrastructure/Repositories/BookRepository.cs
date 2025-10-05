using AutoMapper;
using BookManagement.Domain.Repositories;
using BookManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookManagementDbContext _context;
        private readonly IMapper _mapper;

        public BookRepository(BookManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Domain.Entities.Book>> GetAsync(CancellationToken cancellationToken = default)
        {
            var rawBooks = await _context.Books.ToListAsync(cancellationToken);
            var books = _mapper.Map<List<Domain.Entities.Book>>(rawBooks);

            return books;
        }
        

        public async Task<Domain.Entities.Book> GetByIdAsync(int bookId, CancellationToken cancellationToken = default)
        {
            var rawBook = await _context.Books.SingleOrDefaultAsync(book => book.BookId == bookId);
            
            return _mapper.Map<Domain.Entities.Book>(rawBook);
        }

        public void Insert(Domain.Entities.Book book)
        {
            var rawBook = _mapper.Map<Entities.Book>(book);
            _context.Books.Add(rawBook);
        }
    }
}
