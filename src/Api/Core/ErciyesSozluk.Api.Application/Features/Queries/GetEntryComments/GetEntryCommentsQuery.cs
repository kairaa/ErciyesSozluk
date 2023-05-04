using ErciyesSozluk.Common.Models.Page;
using ErciyesSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Queries.GetEntryComments
{
    public class GetEntryCommentsQuery : BasePagedQuery, IRequest<PagedViewModel<GetEntryCommentsViewModel>>
    {

        public Guid EntryId { get; set; }

        public Guid? UserId { get; set; }

        public GetEntryCommentsQuery(Guid entryId, Guid? userId, int page, int pageSize) : base(page, pageSize)
        {
            EntryId = entryId;
            UserId = userId;
        }
    }
}
