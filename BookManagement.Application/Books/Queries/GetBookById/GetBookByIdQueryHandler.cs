using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Contracts.Books;
using BookManagement.Domain.Exceptions;
using BookManagement.Domain.Repositories;
using Mapster;

namespace BookManagement.Application.Books.Queries.GetBookById;

public sealed class GetBookByIdQueryHandler : IQueryHandler<GetBookByIdQuery, BookResponse>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdQueryHandler(IBookRepository bookRepository) => _bookRepository = bookRepository;

    public async Task<BookResponse> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);

        if (book == null)
            throw new UserNotFoundException(request.BookId);

        return book.Adapt<BookResponse>();
    }
}
