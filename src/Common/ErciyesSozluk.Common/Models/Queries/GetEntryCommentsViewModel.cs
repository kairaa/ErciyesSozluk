using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Common.Models.Queries
{
    public class GetEntryCommentsViewModel : BaseFooterRateFavoritedViewModel
    {
        //BaseFooterRateFavoritedViewModel'i miras aldigi icin vote ve fav ile ilgili bilgilere sahip olur
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
    }
}
