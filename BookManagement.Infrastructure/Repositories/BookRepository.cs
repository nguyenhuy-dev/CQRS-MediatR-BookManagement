using AutoMapper;
using BookManagement.Domain.Repositories;
using BookManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Communicate with Book table at database.
    /// </summary>
    /// <seealso cref="BookManagement.Domain.Repositories.IBookRepository" />
    public class BookRepository : IBookRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly BookManagementDbContext _context;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="mapper">The mapper.</param>
        public BookRepository(BookManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<Domain.Entities.Book>> GetAsync(CancellationToken cancellationToken = default)
        {
            var rawBooks = await _context.Books.ToListAsync(cancellationToken);
            var books = _mapper.Map<List<Domain.Entities.Book>>(rawBooks);

            return books;
        }


        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Domain.Entities.Book> GetByIdAsync(int bookId, CancellationToken cancellationToken = default)
        {
            var rawBook = await _context.Books.SingleOrDefaultAsync(book => book.BookId == bookId);
            
            return _mapper.Map<Domain.Entities.Book>(rawBook);
        }

        /// <summary>
        /// Inserts the specified book.
        /// </summary>
        /// <param name="book">The book.</param>
        public void Insert(Domain.Entities.Book book)
        {
            var rawBook = _mapper.Map<Entities.Book>(book);
            _context.Books.Add(rawBook);
        }
    }
}
