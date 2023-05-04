using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Common.Models.Page
{
    public class BasePagedQuery
    {
        //bu sınıfı implemente eden diğer sınıflar otomatik olarak sayfalamaya sahip olur

        //hangi sayfada olunduğunu tutar
        public int Page { get; set; }
        public int PageSize { get; set; }

        public BasePagedQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
