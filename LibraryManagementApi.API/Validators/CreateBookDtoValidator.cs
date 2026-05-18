using FluentValidation;
using LibraryManagementApi.API.DTOs;

namespace LibraryManagementApi.API.Validators;

public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Author)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ISBN)
            .NotEmpty()
            .Length(10, 20);

        RuleFor(x => x.PublishedYear)
            .InclusiveBetween(1800, DateTime.UtcNow.Year);

        RuleFor(x => x.AvailableCopies)
            .GreaterThanOrEqualTo(0);
    }
}