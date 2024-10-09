using AspireTodo.Core.Shared;
using FluentValidation;

namespace AspireTodo.UserManagement.Shared;

public class RegisterRequest
{
    public string Name { get; set; }
    public string Family { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name).MaximumLength(GlobalConstants.NameMaxLength).NotEmpty().NotNull();
        RuleFor(x => x.Family).MaximumLength(GlobalConstants.FamilyMaxLength).NotEmpty().NotNull();
        RuleFor(x => x.PhoneNumber).MaximumLength(GlobalConstants.PhoneNumberMaxLength).NotEmpty().NotNull();
        RuleFor(x => x.Password).NotEmpty()
            .MinimumLength(GlobalConstants.MinPasswordLength)
            .NotNull();
    }
}