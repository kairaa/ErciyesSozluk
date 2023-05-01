using ErciyesSozluk.Common.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(i => i.EmailAddress).NotNull()
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("{PropertyName} not a valid email address");

            RuleFor(i => i.Password).NotNull()
                .MinimumLength(6)
                .WithMessage("{PropertyName} should at least be {MinLength} character");

            RuleFor(i => i.UserName).NotNull()
                .MinimumLength(6)
                .WithMessage("{PropertyName} should at least be {MinLength} character");
        }
    }
}
