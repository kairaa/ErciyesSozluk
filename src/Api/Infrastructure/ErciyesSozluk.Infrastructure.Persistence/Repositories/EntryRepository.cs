using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Api.Domain.Models;
using ErciyesSozluk.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Infrastructure.Persistence.Repositories
{
    public class EntryRepository : GenericRepository<Entry>, IEntryRepository
    {
        public EntryRepository(ErciyesSozlukContext dbContext) : base(dbContext)
        {
        }
    }
}
