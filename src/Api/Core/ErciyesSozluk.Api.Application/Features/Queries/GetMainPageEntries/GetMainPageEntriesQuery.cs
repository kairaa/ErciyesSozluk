using ErciyesSozluk.Common.Models.Page;
using ErciyesSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Queries.GetMainPageEntries
{
    public class GetMainPageEntriesQuery : BasePagedQuery, IRequest<PagedViewModel<GetEntryDetailViewModel>>
    {
        //giriş yapmış olan kullanıcının id'si
        public Guid? UserId { get; set; }

        public GetMainPageEntriesQuery(Guid? userId, int page, int pageSize) : base(page, pageSize)
        {
            UserId = userId;
        }
    }
}
