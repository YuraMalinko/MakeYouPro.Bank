using MakeYouPro.Bourse.CRM.Auth.Dal.Context;
using MakeYouPro.Bourse.CRM.Auth.Dal.IRepository;
using MakeYouPro.Bourse.CRM.Auth.Dal.Models;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bourse.CRM.Auth.Dal.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> AddUserAsync(UserEntity user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var newUser = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == user.Id);

            if (newUser == null)
            {
                throw new WritingDataToServerException($"Create user {user} was not executed");
            }

            return newUser;
        }

        public async Task<UserEntity> UpdateUserAsync(UserEntity updateUser)
        {
            _context.Users.Update(updateUser);
            await _context.SaveChangesAsync();

            var result = await _context.Users
                .Include(u => u.RefreshTokens)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Equals(updateUser));

            if (result == null)
            {
                throw new WritingDataToServerException($"The update {updateUser} was not executed");
            }

            return result;
        }

        public async Task<UserEntity> GetUserByToken(string token)
        {
            var result = await _context.Users
                .Include(u => u.RefreshTokens)
                .AsNoTracking()
                .SingleAsync(u => u.RefreshTokens
                .Any(t => t.Token == token))!;

            return result;
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return (await _context.Users
                .Include (u => u.RefreshTokens)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == email))!;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return await _context.Users
                    .AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UserDestruction(UserEntity user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            if (await _context.Users.AnyAsync(u => u.Id == user.Id))
            {
                throw new WritingDataToServerException($"The deleted {user} was not executed");
            }

            return true;
        }
    }
}
