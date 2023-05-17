using ErciyesSozluk.Common.Models.RequestModels;

namespace ErciyesSozluk.WebApp.Infrastructure.Services.Interfaces
{
    public interface IIdentityService
    {
        bool IsLoggedIn { get; }

        string GetUserToken();

        string GetUserName();

        Guid GetUserId();

        Task<bool> Login(LoginUserCommand command);

        void Logout();

        //return type degisebilir
        Task<bool> Register(CreateUserCommand command);
    }
}
