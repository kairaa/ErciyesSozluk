using ErciyesSozluk.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Common.Models.Queries
{
    public class BaseFooterRateViewModel
    {
        //bundan üretilen viewmodel otomatik olarak voteetype'a sahip olur
        public VoteType VoteType { get; set; }
    }

    public class BaseFooterFavoritedViewModel
    {
        public bool IsFavorited { get; set; }

        public int FavoritedCount { get; set; }
    }

    public class BaseFooterRateFavoritedViewModel : BaseFooterFavoritedViewModel
    {
        //hem vote ile ilgili bilgiler hem de fav ile ilgili bilgileri icerir
        public VoteType VoteType { get; set; }
    }
}
