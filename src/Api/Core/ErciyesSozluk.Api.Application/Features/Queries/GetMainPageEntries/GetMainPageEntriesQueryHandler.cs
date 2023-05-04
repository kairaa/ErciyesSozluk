using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Common.Infrastructure.Extensions;
using ErciyesSozluk.Common.Models.Page;
using ErciyesSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Queries.GetMainPageEntries
{
    //geriye sayfalanmış şekilde entry'ler döndürülür
    public class GetMainPageEntriesQueryHandler : IRequestHandler<GetMainPageEntriesQuery, PagedViewModel<GetEntryDetailViewModel>>
    {
        private readonly IEntryRepository entryRepository;

        public GetMainPageEntriesQueryHandler(IEntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        public async Task<PagedViewModel<GetEntryDetailViewModel>> Handle(GetMainPageEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = entryRepository.AsQueryable();

            //entry'ye ait favori bilgisi, vote bilgisi ve kullanıcı bilgisi eklenir
            query = query.Include(i => i.EntryFavorites)
                         .Include(i => i.CreatedBy)
                         .Include(i => i.EntryVotes);

            var list = query.Select(i => new GetEntryDetailViewModel()
            {
                Id = i.Id,
                Subject = i.Subject,
                Content = i.Content,
                //giriş yapmış kullanıcı tarafından entry'nin favoriye eklenip eklenilmediğini tutar
                IsFavorited = request.UserId.HasValue && i.EntryFavorites.Any(j => j.CreatedById == request.UserId),
                FavoritedCount = i.EntryFavorites.Count,
                CreatedDate = i.CreateDate,
                CreatedByUserName = i.CreatedBy.UserName,
                //giriş yapmış kullanıcı tarafından entry'nin vote'lanıp vote'lanmadığını tutar
                //giriş yapmış kullanıcı vote'lanış ise vote tipini atar, vote'lamamış ise vote tipinin none yapar
                VoteType =
                    request.UserId.HasValue && i.EntryVotes.Any(j => j.CreatedById == request.UserId)
                    ? i.EntryVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType
                    : Common.ViewModels.VoteType.None
            });

            //yukarıda liste halinde tutulan entry'ler sayfalanmış hale getirilir
            var entries = await list.GetPaged(request.Page, request.PageSize);

            //return new PagedViewModel<GetEntryDetailViewModel>(entries.Results, entries.PageInfo);
            //zaten sayfalama islemi uygulandigi icin yorum satiri haline getirilen isleme gerek yok
            return entries;
        }
    }
}
