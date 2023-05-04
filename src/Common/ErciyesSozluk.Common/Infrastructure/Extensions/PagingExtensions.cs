using ErciyesSozluk.Common.Models.Page;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Common.Infrastructure.Extensions
{
    public static class PagingExtensions
    {
        //this IQueryable<T> query sayesinde IQueryable<T> tipindeki bir değişken "." ile GetPaged metodunu çağırabilir
        //bkz: list.GetPaged(request.Page, request.PageSize);
        public static async Task<PagedViewModel<T>> GetPaged<T>(this IQueryable<T> query,
                                                                int currentPage,
                                                                int pageSize) where T : class
        {
            //toplam kayıt sayısı
            var count = await query.CountAsync();

            Page paging = new(currentPage, pageSize, count);

            //aktif sayfaya gelene kadar atlanacak veri sayısı paging.Skip ile verilir,
            //aktif sayfada gösterilecek olan veri sayısı paging.PageSize ile verilir
            var data = await query.Skip(paging.Skip).Take(paging.PageSize).AsNoTracking().ToListAsync();

            var result = new PagedViewModel<T>(data, paging);

            return result;
        }

    }
}
