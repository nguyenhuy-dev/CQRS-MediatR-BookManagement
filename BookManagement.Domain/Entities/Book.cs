namespace BookManagement.Domain.Entities
{
    public class Book
    {
        public int BookId { get; private set; }
        public string Title { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public int AuthorId { get; set; }
    }
}
