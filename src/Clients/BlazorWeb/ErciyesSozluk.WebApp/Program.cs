using Blazored.LocalStorage;
using ErciyesSozluk.WebApp;
using ErciyesSozluk.WebApp.Infrastructure.Services.Interfaces;
using ErciyesSozluk.WebApp.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ErciyesSozluk.WebApp.Infrastructure.Auth;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddHttpClient("WebApiClient", client =>
{
    //base adres sayesinde istek atilan api'leri basina bu adres eklenir
    //orn. DeleteEntryVote için istek atilan adres https://localhost:5001/api/Vote/DeleteEntryVote/{entryId} olur
    client.BaseAddress = new Uri("https://localhost:5001");
})
    .AddHttpMessageHandler<AuthTokenHandler>(); //TODO: AuthTokenHandler will be here

builder.Services.AddScoped(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return clientFactory.CreateClient("WebApiClient");
});

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AuthTokenHandler>();

builder.Services.AddTransient<IEntryService, EntryService>();
builder.Services.AddTransient<IFavService, FavService>();
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IVoteService, VoteService>();


builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
