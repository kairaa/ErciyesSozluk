using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Common.Models.Queries
{
    //BaseFooterRateFavoritedViewModel sayesinde vote ve favorite hakkındaki bilgileri de implemente ederiz
    public class GetEntryDetailViewModel : BaseFooterRateFavoritedViewModel
    {
        //başlık sayfasındaki her bir entry için tutulan bilgiler
        public Guid Id { get; set; }

        public string Subject { get; set; }
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedByUserName { get; set; }
    }
}
