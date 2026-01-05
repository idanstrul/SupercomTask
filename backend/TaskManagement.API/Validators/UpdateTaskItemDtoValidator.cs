using FluentValidation;
using TaskManagement.API.DTOs;

namespace TaskManagement.API.Validators;

public class UpdateTaskItemDtoValidator : AbstractValidator<UpdateTaskItemDto>
{
    public UpdateTaskItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required")
            .Must(ValidationUtilities.BeValidDate).WithMessage("Due date must be a valid date");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Priority must be a valid value (Low, Medium, High)");
    }
}
