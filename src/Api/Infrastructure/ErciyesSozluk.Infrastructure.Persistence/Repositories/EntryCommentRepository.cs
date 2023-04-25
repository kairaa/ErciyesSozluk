using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Api.Domain.Models;
using ErciyesSozluk.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Infrastructure.Persistence.Repositories
{
    public class EntryCommentRepository : GenericRepository<EntryComment>, IEntryCommentRepository
    {
        public EntryCommentRepository(ErciyesSozlukContext dbContext) : base(dbContext)
        {
        }
    }
}
