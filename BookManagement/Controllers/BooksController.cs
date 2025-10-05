using BookManagement.Application.Books.Commands.CreateBook;
using BookManagement.Application.Books.Queries.GetBookById;
using BookManagement.Application.Books.Queries.GetBooks;
using BookManagement.Application.Contracts.Books;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ISender _sender;

        public BooksController(ISender sender) => _sender = sender;

        [HttpGet]
        [ProducesResponseType(typeof(List<BookResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooks(CancellationToken cancellationToken)
        {
            var query = new GetBooksQuery();

            var books = await _sender.Send(query, cancellationToken);

            return Ok(books);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BookResponseCreated), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request, CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateBookCommand>();

            var book = await _sender.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetBookById), new { bookId = book.BookId }, book);
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookById(int bookId, CancellationToken cancellationToken)
        {
            var query = new GetBookByIdQuery(bookId);

            var book = await _sender.Send(query, cancellationToken);

            return Ok(book);
        }
    }
}
