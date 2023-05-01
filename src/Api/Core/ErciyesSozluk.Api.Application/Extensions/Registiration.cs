using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ErciyesSozluk.Api.Application.Extensions
{
    public static class Registiration
    {
        public static IServiceCollection AddApplicationRegistiration(this IServiceCollection services)
        {
            //webapi'ye bağlı olan tüm class library'leri içerir
            var assm = Assembly.GetExecutingAssembly();

            //https://stackoverflow.com/questions/75527541/could-not-load-type-mediatr-servicefactory
            //services.AddMediatR(assm) bu şekilde kullanılınca hata veriyor, yukarıdaki linkteki çözüm uygulandı
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assm));
            services.AddAutoMapper(assm);
            services.AddValidatorsFromAssembly(assm);

            return services;
        }
    }
}
