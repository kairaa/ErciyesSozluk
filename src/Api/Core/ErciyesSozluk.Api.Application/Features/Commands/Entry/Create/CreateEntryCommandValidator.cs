using ErciyesSozluk.Common.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Commands.Entry.Create
{
    public class CreateEntryCommandValidator : AbstractValidator<CreateEntryCommand>
    {
        public CreateEntryCommandValidator()
        {
            RuleFor(i => i.Subject).NotNull()
                .MinimumLength(10)
                .WithMessage("{PropertyName} should at least be {MinLength} character");

            RuleFor(i => i.Content).NotNull()
                .MinimumLength(10)
                .WithMessage("{PropertyName} should at least be {MinLength} character");
        }
    }
}
