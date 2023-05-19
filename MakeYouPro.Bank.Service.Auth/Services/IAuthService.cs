using MakeYouPro.Bank.Service.Auth.Models;

namespace MakeYouPro.Bank.Service.Auth.Services
{
    public interface IAuthService
    {
        Task<User> RegisterUserAsync(User userregister);
    }
}
