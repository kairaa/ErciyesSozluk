using ErciyesSozluk.Common.BlazorSozluk.Common;
using ErciyesSozluk.Common.Events.Entry;
using ErciyesSozluk.Common.Infrastructure;
using ErciyesSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Commands.Entry.CreateVote
{
    public class CreateEntryVoteCommandHandler : IRequestHandler<CreateEntryVoteCommand, bool>
    {
        public async Task<bool> Handle(CreateEntryVoteCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.VoteExchangeName,
                    exchangeType: SozlukConstants.DefaultExchangeType,
                    queueName: SozlukConstants.CreateEntryVoteQueueName,
                    obj: new CreateEntryVoteEvent()
                    {
                        EntryId = request.EntrytId,
                        CreatedBy = request.CreatedBy,
                        VoteType = request.VoteType
                    });
            return await Task.FromResult(true);
        }
    }
}
