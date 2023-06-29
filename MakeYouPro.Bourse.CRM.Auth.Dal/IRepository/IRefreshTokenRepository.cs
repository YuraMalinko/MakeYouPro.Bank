using MakeYouPro.Bourse.CRM.Auth.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Auth.Dal.IRepository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokenEntity> AddRefreshToken(RefreshTokenEntity refreshToken);

        Task<bool> RevokeToken(string token);
    }
}