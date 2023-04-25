using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Infrastructure.Persistence.Context;
using ErciyesSozluk.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Infrastructure.Persistence.Extensions
{
    public static class Registiration
    {
        public static IServiceCollection AddInfrastructureRegistiration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context.ErciyesSozlukContext>(conf =>
            {
                string connStr = configuration["ErciyesSozlukDbConnectionString"].ToString();
                conf.UseSqlServer(connStr, opt =>
                {
                    //baglanti sirasinda bir hata meydana gelirse retry mekanizmasi devreye girer
                    opt.EnableRetryOnFailure();
                });
            });

            //fake data'lar bir kere veri tabanına atıldıktan sonra atılmasına gerek yok
            //bundan dolayı bu iki satır yorum satırı yapılır
            //var seedData = new SeedData();
            //seedData.SeedAsync(configuration).GetAwaiter().GetResult();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IEntryCommentRepository, EntryCommentRepository>();

            return services;
        }
    }
}
