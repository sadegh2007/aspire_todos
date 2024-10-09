using AspireTodo.Core.Shared;
using FluentValidation;

namespace AspireTodo.UserManagement.Shared;

public class LoginRequest
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
        RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(GlobalConstants.MinPasswordLength);
    }
}