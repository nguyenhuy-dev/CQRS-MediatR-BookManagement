namespace BookManagement.Application.Books.Commands.CreateBook;

public sealed record CreateBookRequest(string Title, int PublishedYear, int AuthorId);
