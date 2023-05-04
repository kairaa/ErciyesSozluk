using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Queries.SearchBySubject
{
    public class SearchEntryQueryHandler : IRequestHandler<SearchEntryQuery, List<SearchEntryViewModel>>
    {
        private readonly IEntryRepository entryRepository;

        public SearchEntryQueryHandler(IEntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        public async Task<List<SearchEntryViewModel>> Handle(SearchEntryQuery request, CancellationToken cancellationToken)
        {
            // TODO validation, request.SearchText length should be checked

            //girilen keyword viretabaında LIKE ile aranır
            var result = entryRepository
                //kullanılan veri tabanının özel LIKE komutu çalıştırılır
                //girilen searchText'i içeren tüm başlıklar listelenir
                //sadece girilen searchText ile başlayan başlıklar listelenecekse baştaki % silinmeli
                .Get(i => EF.Functions.Like(i.Subject, $"%{request.SearchText}%"))
                .Select(i => new SearchEntryViewModel()
                {
                    Id = i.Id,
                    Subject = i.Subject,
                });

            return await result.ToListAsync(cancellationToken);
        }
    }
}
