using MakeYouPro.Bourse.CRM.Auth.Bll.Models;

namespace MakeYouPro.Bourse.CRM.Auth.Bll.IServices
{
    public interface IAuthService
    {
        Task<AuthResult> Authenticate(User user);

        Task<AuthResult> RefreshToken(string token);

        Task<bool> RevokeRefreshToken(string token);

        Task<AuthResult> UpdatePassword(User user);
    }
}