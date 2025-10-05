using FluentValidation;

namespace BookManagement.Application.Books.Commands.CreateBook
{
    public sealed class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.Title).MaximumLength(50);

            RuleFor(x => x.PublishedYear)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("'PublishedYear' must be valid.");

            RuleFor(x => x.AuthorId)
                .NotEmpty()
                .Must(BeAValidAuthorId).WithMessage("'AuthorId' must be existed.");
        }

        private bool BeAValidAuthorId(int authorId) => authorId > 0;
    }
}
