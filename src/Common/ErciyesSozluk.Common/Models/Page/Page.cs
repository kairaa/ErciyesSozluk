using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Common.Models.Page
{
    public class Page
    {
        public int CurrentPage { get; set; }

        //bir sayfada gösterilecek olan entry sayısı
        public int PageSize { get; set; }

        //toplam veri/entry sayısı
        public int TotalRowCount { get; set; }

        public int TotalPageCount => (int)Math.Ceiling((double)TotalRowCount / PageSize);

        //Örneğin birinci sayfadan üçüncü sayfaya geçerken atlanacak olan entry sayısı
        public int Skip => (CurrentPage - 1) * PageSize;

        public Page(int currentPage, int pageSize, int totalRowCount)
        {
            if (currentPage < 1)
                throw new ArgumentException("Invalid page number!");

            if (pageSize < 1)
                throw new ArgumentException("Invalid page size!");

            TotalRowCount = totalRowCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public Page(int pageSize, int totalRowCount) :
            this(1, pageSize, totalRowCount)
        {

        }

        public Page(int totalRowCount) :
            this(1, 10, totalRowCount)
        {

        }

        public Page() : this(0)
        {

        }
    }
}
