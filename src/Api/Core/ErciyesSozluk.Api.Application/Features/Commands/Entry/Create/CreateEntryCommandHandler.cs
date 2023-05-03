using AutoMapper;
using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Commands.Entry
{
    public class CreateEntryCommandHandler : IRequestHandler<CreateEntryCommand, Guid>
    {
        private readonly IEntryRepository entryRepository;
        private readonly IMapper mapper;

        public CreateEntryCommandHandler(IEntryRepository entryRepository, IMapper mapper)
        {
            this.entryRepository = entryRepository;
            this.mapper = mapper;
        }
        public async Task<Guid> Handle(CreateEntryCommand request, CancellationToken cancellationToken)
        {
            var dbEntry = mapper.Map<Domain.Models.Entry>(request);

            await entryRepository.AddAsync(dbEntry);

            return dbEntry.Id;
        }
    }
}
