using MakeYouPro.Bourse.CRM.Core.Clients.AuthService.Models;

namespace MakeYouPro.Bourse.CRM.Core.Clients.AuthService
{
    public interface IAuthServiceClient
    {
        Task RegisterAsync(UserRegisterRequest user);

        Task<UserRegisterRequest> Login(UserRegisterRequest request);
    }
}