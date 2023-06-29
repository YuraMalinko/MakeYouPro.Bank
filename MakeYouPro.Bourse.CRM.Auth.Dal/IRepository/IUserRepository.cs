using MakeYouPro.Bourse.CRM.Auth.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Auth.Dal.IRepository
{
    public interface IUserRepository
    {
        Task<UserEntity> AddUserAsync(UserEntity user);

        Task<UserEntity> GetUserByEmailAsync(string email);

        Task<UserEntity> UpdateUserAsync(UserEntity updateUser);

        Task<UserEntity> GetUserByToken(string token);

        Task<bool> CheckEmailAsync(string email);

        Task<bool> UserDestruction(UserEntity user);
    }
}
