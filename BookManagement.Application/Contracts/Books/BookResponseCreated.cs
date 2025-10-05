namespace BookManagement.Application.Contracts.Books;

public sealed record BookResponseCreated(int BookId, string Title, int PublishedYear, int AuthorId);