using ErciyesSozluk.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Common.Events.EntryComment
{
    public class DeleteEntryCommentFavEvent
    {
        public Guid EntryCommentId { get; set; }

        public VoteType VoteType { get; set; }

        public Guid CreatedBy { get; set; }
    }
}
