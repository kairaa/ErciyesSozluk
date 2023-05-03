using ErciyesSozluk.Common.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Common.Models.RequestModels
{
    public class CreateEntryVoteCommand : IRequest<bool>
    {
        public Guid EntrytId { get; set; }

        public VoteType VoteType { get; set; }

        public Guid CreatedBy { get; set; }

        public CreateEntryVoteCommand()
        {

        }

        public CreateEntryVoteCommand(Guid entryId, VoteType voteType, Guid createdBy)
        {
            EntrytId = entryId;
            VoteType = voteType;
            CreatedBy = createdBy;
        }
    }
}
