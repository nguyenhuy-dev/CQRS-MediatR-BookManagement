using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Contracts.Books;

namespace BookManagement.Application.Books.Queries.GetBooks;

public sealed record GetBooksQuery() : IQuery<List<BookResponse>>;