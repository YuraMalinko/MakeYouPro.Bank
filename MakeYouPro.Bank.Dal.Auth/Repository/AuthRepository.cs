using MakeYouPro.Bank.Dal.Auth.Context;
using MakeYouPro.Bank.Dal.Auth.Models;

namespace MakeYouPro.Bank.Dal.Auth.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private static AuthContext _context;
        public AuthRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<UserDal> AddUserAsync(UserDal user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return _context.Users
                .Single(u => u.Id == user.Id);
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return _context.Users
                    .ToList()
                    .Any(u => u.Email == email);
        }
    }
}
