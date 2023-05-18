using Blazored.LocalStorage;
using ErciyesSozluk.WebApp.Infrastructure.Extensions;
using ErciyesSozluk.WebApp.Infrastructure.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ErciyesSozluk.WebApp.Infrastructure.Auth
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        //kullanıcın authenticate olup olmadığını kontrol eder
        private readonly ILocalStorageService localStorageService;
        //sisteme giriş yapan kullanıcı blinmiyorsa anonymoıs seçilir
        private readonly AuthenticationState anonymous;

        public AuthStateProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
            //bilinmeyen kullanıcı tanımlanır
            this.anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //bu bir jwt token'dır. bundan dolayı claims'leri içerir
            var apiToken = await localStorageService.GetToken();

            if (string.IsNullOrEmpty(apiToken))
            {
                //token yok ise giriş yapan bir kullanıcı yoktur, anonymous döner
                return anonymous;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(apiToken);

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(securityToken.Claims, "jwtAuthType"));

            return new AuthenticationState(claimsPrincipal);
        }

        public void NotifyUserLogin(string userName, Guid userId)
        {
            //kullanıcı login olduğumda claim'ler elle verilir
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "jwtAuthType"));

            var authState = Task.FromResult(new AuthenticationState(claimsPrincipal));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            //logout olduğu zaman sisteme giriş yapan kullanıcı anonymous olarak değiştirilir
            var authState = Task.FromResult(anonymous);

            NotifyAuthenticationStateChanged(authState);
        }
    }
}