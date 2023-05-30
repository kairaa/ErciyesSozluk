using Blazored.LocalStorage;
using ErciyesSozluk.Common.Infrastructure.Exceptions;
using ErciyesSozluk.Common.Infrastructure.Results;
using ErciyesSozluk.Common.Models.Queries;
using ErciyesSozluk.Common.Models.RequestModels;
using ErciyesSozluk.WebApp.Infrastructure.Auth;
using ErciyesSozluk.WebApp.Infrastructure.Extensions;
using ErciyesSozluk.WebApp.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json;

namespace ErciyesSozluk.WebApp.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private JsonSerializerOptions defaultJsonOpt => new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient httpClient;
        private readonly ISyncLocalStorageService syncLocalStorageService;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public IdentityService(HttpClient httpClient, ISyncLocalStorageService syncLocalStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.syncLocalStorageService = syncLocalStorageService;
            this.authenticationStateProvider = authenticationStateProvider;
        }


        public bool IsLoggedIn => !string.IsNullOrEmpty(GetUserToken());

        public string GetUserToken()
        {
            return syncLocalStorageService.GetToken();
        }

        public string GetUserName()
        {
            return syncLocalStorageService.GetToken();
        }

        public Guid GetUserId()
        {
            return syncLocalStorageService.GetUserId();
        }

        public async Task<bool> Login(LoginUserCommand command)
        {
            string responseStr;
            var httpResponse = await httpClient.PostAsJsonAsync("/api/User/Login", command);

            if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    responseStr = await httpResponse.Content.ReadAsStringAsync();
                    var validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr, defaultJsonOpt);
                    responseStr = validation.FlattenErrors;
                    throw new DatabaseValidationException(responseStr);
                }

                return false;
            }


            responseStr = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<LoginUserViewModel>(responseStr);

            if (!string.IsNullOrEmpty(response.Token)) // login success
            {
                syncLocalStorageService.SetToken(response.Token);
                syncLocalStorageService.SetUsername(response.UserName);
                syncLocalStorageService.SetUserId(response.Id);

                //TODO Check after auth
                ((AuthStateProvider)authenticationStateProvider).NotifyUserLogin(response.UserName, response.Id);

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", response.UserName);

                return true;
            }

            return false;
        }

        public async Task<bool> Register(CreateUserCommand command)
        {
            //TODO: implement this register method
            //TODO: check this method
            var httpResponse = await httpClient.PostAsJsonAsync("/api/User/Register", command);
            return httpResponse.IsSuccessStatusCode;
        }

        public void Logout()
        {
            syncLocalStorageService.RemoveItem(LocalStorageExtension.TokenName);
            syncLocalStorageService.RemoveItem(LocalStorageExtension.UserName);
            syncLocalStorageService.RemoveItem(LocalStorageExtension.UserId);

            // TODO Check after auth
            ((AuthStateProvider)authenticationStateProvider).NotifyUserLogout();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}

/*
 * public async Task<bool> Register(CreateUserCommand command)
        {
            //TODO: implement this register method
            //TODO: check this method
            var httpResponse = await httpClient.PostAsJsonAsync("/api/User/Register", command);
            return httpResponse.IsSuccessStatusCode;
        }
 */