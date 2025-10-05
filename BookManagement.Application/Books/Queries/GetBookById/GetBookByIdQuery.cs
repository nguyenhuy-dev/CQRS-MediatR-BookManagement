using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Contracts.Books;

namespace BookManagement.Application.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(int BookId) : IQuery<BookResponse>;