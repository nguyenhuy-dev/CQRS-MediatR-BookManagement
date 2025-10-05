using System.Text.Json.Serialization;

namespace BookManagement.Infrastructure.Entities;

public class Book
{
    public int BookId { get; set; }
    public string? Title { get; set; }
    public int PublishedYear { get; set; }
    public int AuthorId { get; set; }
    [JsonIgnore]
    public Author Author { get; set; } = default!;
}
