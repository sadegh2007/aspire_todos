using AspireTodo.Core.Shared;
using FluentValidation;

namespace AspireTodo.UserManagement.Shared;

public class UpdateProfileRequest
{
    public string Name { get; set; }
    public string Family { get; set; }
}

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(GlobalConstants.NameMaxLength);
        RuleFor(x => x.Family).NotEmpty().NotNull().MaximumLength(GlobalConstants.FamilyMaxLength);
    }
}