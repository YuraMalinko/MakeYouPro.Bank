using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Users.Request;

namespace MakeYouPro.Bourse.CRM.Api.Validations
{
    public class UpdatePasswordValidatior : AbstractValidator<UserUpdateRequest>
    {
        public UpdatePasswordValidatior()
        {
            RuleFor(u => u.Email.Trim()).NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email is required");
            RuleFor(u => u.Password).NotEmpty()
                .WithMessage("Password is required");
            RuleFor(u => u.UpdateUserPassword)
                .NotEmpty()
                .WithMessage("Update password is required")
                .MinimumLength(6)
                .WithMessage("Password must not be less than 6 characters")
                .Matches(@"[A-Z]+")
                .WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+")
                .WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+")
                .WithMessage("Your password must contain at least one number.");
        }
    }
}