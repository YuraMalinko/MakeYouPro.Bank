using MakeYouPro.Bank.Dal.Auth.Models;

namespace MakeYouPro.Bank.Dal.Auth.Repository
{
    public interface IAuthRepository
    {
        Task<UserDal> AddUserAsync(UserDal user);

        Task<bool> CheckEmailAsync(string email);
    }
}
