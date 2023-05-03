using ErciyesSozluk.Common.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Commands.EntryComment.Create
{
    public class CreateEntryCommentCommandValidator : AbstractValidator<CreateEntryCommentCommand>
    {
        public CreateEntryCommentCommandValidator()
        {
            RuleFor(i => i.Content).NotNull()
                .MinimumLength(10)
                .WithMessage("{PropertyName} should at least be {MinLength} character");
        }
    }
}
