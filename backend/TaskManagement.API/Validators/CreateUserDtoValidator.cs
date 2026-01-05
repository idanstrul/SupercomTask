using FluentValidation;
using TaskManagement.API.DTOs;

namespace TaskManagement.API.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(200).WithMessage("Full name must not exceed 200 characters");

        RuleFor(x => x.Telephone)
            .NotEmpty().WithMessage("Telephone is required")
            .Must(ValidationUtilities.BeValidTelephone).WithMessage("Telephone must be in a valid format");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be in a valid format")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters");
    }
}
