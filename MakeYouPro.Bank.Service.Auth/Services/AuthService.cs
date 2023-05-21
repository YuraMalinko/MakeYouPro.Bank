using AutoMapper;
using MakeYouPro.Bank.Dal.Auth.Models;
using MakeYouPro.Bank.Dal.Auth.Repository;
using MakeYouPro.Bank.Service.Auth.Models;

namespace MakeYouPro.Bank.Service.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;

        public AuthService(IMapper mapper, IAuthRepository authRepository)
        {
            _mapper = mapper;
            _authRepository = authRepository;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            if (!await _authRepository.CheckEmailAsync(user.Email))
            {
                var userDal = _mapper.Map<UserDal>(user);

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDal.Password);
                userDal.Password = passwordHash;

                var callback = await _authRepository.AddUserAsync(userDal);
                var result = _mapper.Map<User>(callback);

                return result;
            }
            else
            {
                throw new Exception($"Что-то пошло не так");
            }
        }
    }
}
