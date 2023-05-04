using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Common.Models.Queries
{
    public class GetEntriesViewModel
    {
        //dışarıya dönülecek model
        //örneğin ekşisözlük'te sol taraftaki her bir başlık için tutulan bilgiler
        //gidilecek başlığın id'si
        public Guid Id { get; set; }
        public string Subject { get; set; }

        //başlığa girilen entry sayısı
        public int CommentCount { get; set; }
    }
}
