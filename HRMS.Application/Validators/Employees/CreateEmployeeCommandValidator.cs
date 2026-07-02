using FluentValidation;
using HRMS.Application.Features.Employees.Commands;

namespace HRMS.Application.Validators.Employees
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be 10 digits.");

            RuleFor(x => x.Salary)
                .GreaterThan(0).WithMessage("Salary must be greater than 0.");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Valid Department is required.");

            RuleFor(x => x.DateOfJoining)
                .NotEmpty().WithMessage("Date of joining is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date of joining cannot be in the future.");
        }
    }
}