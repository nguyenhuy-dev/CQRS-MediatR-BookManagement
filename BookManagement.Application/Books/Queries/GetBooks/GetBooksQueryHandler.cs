using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Contracts.Books;
using BookManagement.Domain.Repositories;
using Mapster;

namespace BookManagement.Application.Books.Queries.GetBooks
{
    public sealed class GetBooksQueryHandler : IQueryHandler<GetBooksQuery, List<BookResponse>>
    {
        private readonly IBookRepository _bookRepository;

        public GetBooksQueryHandler(IBookRepository bookRepository) => _bookRepository = bookRepository;

        public async Task<List<BookResponse>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAsync(cancellationToken);

            return books.Adapt<List<BookResponse>>();
        }
    }
}
