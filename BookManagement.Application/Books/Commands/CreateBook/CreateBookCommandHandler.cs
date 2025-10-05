using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Contracts.Books;
using BookManagement.Domain.Repositories;
using Mapster;

namespace BookManagement.Application.Books.Commands.CreateBook
{
    public sealed class CreateBookCommandHandler : ICommandHandler<CreateBookCommand, BookResponseCreated>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BookResponseCreated> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Domain.Entities.Book()
            {
                Title = request.Title,
                PublishedYear = request.PublishedYear,
                AuthorId = request.AuthorId
            };

            _bookRepository.Insert(book);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return book.Adapt<BookResponseCreated>();
        }
    }
}