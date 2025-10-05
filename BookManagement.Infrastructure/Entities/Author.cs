using System.Text.Json.Serialization;

namespace BookManagement.Infrastructure.Entities;

public class Author
{
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public int? Dob { get; set; }
    [JsonIgnore]
    public ICollection<Book> Books { get; set; } = [];
}
