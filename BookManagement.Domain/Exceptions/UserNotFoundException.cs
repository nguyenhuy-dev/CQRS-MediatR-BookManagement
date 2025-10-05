namespace BookManagement.Domain.Exceptions
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(int bookId) : base($"The book with the identifier {bookId} was not found.")
        {
        }
    }
}
