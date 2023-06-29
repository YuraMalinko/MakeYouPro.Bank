using MakeYouPro.Bourse.CRM.Auth.Dal.Context;
using MakeYouPro.Bourse.CRM.Auth.Dal.IRepository;
using MakeYouPro.Bourse.CRM.Auth.Dal.Models;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bourse.CRM.Auth.Dal.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly UserContext _context;
        public RefreshTokenRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<RefreshTokenEntity> AddRefreshToken(RefreshTokenEntity refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            var addRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(t => t.Id == refreshToken.Id);

            if (addRefreshToken == null)
            {
                throw new RefreshTokenException("For some reason, the token was not created");
            }

            return addRefreshToken!;
        }

        public async Task<bool> RevokeToken(string token)
        {
            var tokenInBase = await _context.RefreshTokens
                .SingleOrDefaultAsync(t => t.Token == token);

            if (token == null)
            {
                throw new NotFoundException(0, nameof(RefreshTokenEntity));
            }

            tokenInBase!.Revoked = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            var revokeToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(t => t.Id == tokenInBase.Id);

            if (revokeToken!.Revoked == null || revokeToken.IsActive == true)
            {
                throw new RefreshTokenException("The token has not been revoked, it is still active");
            }

            return true;
        }
    }
}
