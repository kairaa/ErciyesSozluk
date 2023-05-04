using ErciyesSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Queries.GetEntries
{
    public class GetEntriesQuery : IRequest<List<GetEntriesViewModel>>
    {
        //sadece bugüne ait entry'leri içerecek olan başlıkların gelip gelmeyeceğini tutar
        public bool TodaysEntries { get; set; }

        //ekşisözlük'teki solda kaç tane başlık gösterileceği
        public int Count { get; set; } = 100;
    }
}
