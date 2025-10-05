using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Contracts.Books;

namespace BookManagement.Application.Books.Commands.CreateBook;
public sealed record CreateBookCommand(string Title, int PublishedYear, int AuthorId) : ICommand<BookResponseCreated>;