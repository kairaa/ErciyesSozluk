using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Queries.GetEntries
{
    public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, List<GetEntriesViewModel>>
    {
        private readonly IEntryRepository entryRepository;
        private readonly IMapper mapper;

        public GetEntriesQueryHandler(IEntryRepository entryRepository, IMapper mapper)
        {
            this.entryRepository = entryRepository;
            this.mapper = mapper;
        }

        public async Task<List<GetEntriesViewModel>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
        {
            //burada asqueryable kullanılmasının sebebi gelecek olan verinin filtrelenecek olması
            //asqueryable yerine getall kullanılırsa tablodaki tüm veriler gelir
            var query = entryRepository.AsQueryable();

            if (request.TodaysEntries)
            {
                //sadece bugüne ait entry'ler gelecekse bu query çağırılır
                query = query
                    .Where(i => i.CreateDate >= DateTime.Now.Date)
                    .Where(i => i.CreateDate <= DateTime.Now.AddDays(1).Date);
            }

            //gelecek olan veriye öncelikle başlığın altına girilen entry'leri eklenir
            query = query.Include(i => i.EntryComments)
                //sıralama kriteri değiştirilebilir
                .OrderBy(i => Guid.NewGuid())
                //count sayısı kaç ise soldki menüde o kadar başlık olur
                .Take(request.Count);


            //sql kısmında query ile oluşturulan komut çalıştırılırken select kısmına sadece
            //GetEntriesViewModel içindeki property'ler yazılır
            return await query.ProjectTo<GetEntriesViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
