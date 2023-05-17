using ErciyesSozluk.Api.Application.Extensions;
using ErciyesSozluk.Api.WebApi.Infrastructure.Extensions;
using ErciyesSozluk.Infrastructure.Persistence.Extensions;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .AddFluentValidation();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//application registiration
builder.Services.AddApplicationRegistiration();

//infrastructure registiration
builder.Services.AddInfrastructureRegistiration(builder.Configuration);

//authregistiration eklenir, authorization için eklenir
//ui tarafindaki isler hallolana kadar controller'lara eklenmedi, hallolduktan sonra eklenecek
builder.Services.ConfigureAuth(builder.Configuration);

//cors icin olusturulan policy ve eklenmesi
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//development ise hata mesajinin detaylari alinir, degilse sadece kisa bir bilgilendirme mesaji alinir
app.ConfigureExceptionHandling(includeExceptionDetails: app.Environment.IsDevelopment());

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("MyPolicy");

app.MapControllers();

app.Run();
