using Blazored.LocalStorage;
using ErciyesSozluk.WebApp.Infrastructure.Extensions;
using System.Net.Http.Headers;

namespace ErciyesSozluk.WebApp.Infrastructure.Auth
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ISyncLocalStorageService syncLocalStorageService;

        public AuthTokenHandler(ISyncLocalStorageService syncLocalStorageService)
        {
            this.syncLocalStorageService = syncLocalStorageService;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = syncLocalStorageService.GetToken();

            if (!string.IsNullOrEmpty(token) && request.Headers.Authorization is null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}