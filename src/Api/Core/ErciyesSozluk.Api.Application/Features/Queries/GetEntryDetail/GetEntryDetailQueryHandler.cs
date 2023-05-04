using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Common.Models.Queries;
using ErciyesSozluk.Common.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Queries.GetEntryDetail
{
    public class GetEntryDetailQueryHandler : IRequestHandler<GetEntryDetailQuery, GetEntryDetailViewModel>
    {
        private readonly IEntryRepository entryRepository;

        public GetEntryDetailQueryHandler(IEntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        public async Task<GetEntryDetailViewModel> Handle(GetEntryDetailQuery request, CancellationToken cancellationToken)
        {
            var query = entryRepository.AsQueryable();

            query = query.Include(i => i.EntryVotes)
                .Include(i => i.EntryFavorites)
                .Include(i => i.CreatedBy)
                .Where(i => i.Id == request.EntryId);

            var list = query.Select(i => new GetEntryDetailViewModel
            {
                Id = i.Id,
                Subject = i.Subject,
                Content = i.Content,
                CreatedDate = i.CreateDate,
                FavoritedCount = i.EntryFavorites.Count,
                IsFavorited = request.UserId.HasValue && i.EntryFavorites.Any(j =>
                                    j.CreatedById == request.UserId),
                CreatedByUserName = i.CreatedBy.UserName,
                VoteType =
                request.UserId.HasValue && i.EntryVotes.Any(j => j.CreatedById == request.UserId)
                        ? i.EntryVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType
                        : VoteType.None
            });

            return await list.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
