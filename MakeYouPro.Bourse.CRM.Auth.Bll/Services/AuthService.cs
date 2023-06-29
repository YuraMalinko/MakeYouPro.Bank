using AutoMapper;
using MakeYouPro.Bourse.CRM.Auth.Bll.IServices;
using MakeYouPro.Bourse.CRM.Auth.Dal.IRepository;
using MakeYouPro.Bourse.CRM.Auth.Dal.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ILogger = NLog.ILogger;


namespace MakeYouPro.Bourse.CRM.Auth.Bll.Models
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public AuthService(IMapper mapper,
            IUserRepository userRepository,
            ILogger logger,
            IRefreshTokenRepository refreshTokenRepository,
            IConfiguration config)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
            _refreshTokenRepository = refreshTokenRepository;
            _config = config;
        }

        public async Task<AuthResult> Authenticate(User user)
        {
            _logger.Info($"Start the {user} Authenticate process");

            var userInBase = _mapper.Map<User>(await _userRepository.GetUserByEmailAsync(user.Email));

             if (userInBase != null && await AuthenticationUser(user, userInBase))
            {
                _logger.Info($"{user} has successfully authenticated, we proceed with authorization.");

                var roles = GetRoleAuth(userInBase);
                var tokenJwt = CreateTokenJWT(roles, userInBase);

                _logger.Info($"A jwt token has been created for {user}, the creation of a refresh token has begun");

                var newRefreshToken = CreateRefreshToken();
                newRefreshToken.UserId = userInBase.Id;
                var refreshTokenToBase = _mapper.Map<RefreshTokenEntity>(newRefreshToken);
                var writeRefreshToken = _mapper.Map<RefreshToken>(await _refreshTokenRepository.AddRefreshToken(refreshTokenToBase));

                var authResult = new AuthResult()
                {
                    Token = tokenJwt,
                    TokenRefresh = writeRefreshToken.Token
                };

                _logger.Info($"{user} has successfully passed authentication and authorization, a token has been issued");

                return authResult;
            }
            else
            {
                throw new AuthenticationException($"Authentication failed, invalid email or password. Or user not active");
            }
        }

        public async Task<AuthResult> RefreshToken(string token)
        {
            _logger.Info("The procedure for the reissue of the jwt token has begun");

            var userInBase = _mapper.Map<User>(await _userRepository.GetUserByToken(token));

            if (userInBase == null)
            {
                throw new AuthenticationException("$Authentication failed,user not found");
            }

            if (userInBase.Status != LeadStatusEnum.Active)
            {
                throw new AuthenticationException($"The {userInBase} status does not allow authentication");
            }

            var refreshTokenInBase = userInBase.RefreshTokens.SingleOrDefault(t => t.Token == token);

            if (!refreshTokenInBase!.IsActive)
            {
                throw new RefreshTokenException("Refresh token is not active");
            }

            var roles = GetRoleAuth(userInBase);
            var jwtToken = CreateTokenJWT(roles, userInBase);

            _logger.Info($"A jwt token has been created for {userInBase}, the creation of a refresh token has begun");

            var ExpiresRefreshTokenMinut = Convert.ToInt32(_config.GetSection("RefreshTokenSettings:ExpiresRefreshTokenMinut").Value);

            if (DateTime.UtcNow.AddMinutes(ExpiresRefreshTokenMinut + 1) >= refreshTokenInBase.Expires)
            {
                var revokeToken = await _refreshTokenRepository.RevokeToken(refreshTokenInBase.Token);

                var newRefreshToken = CreateRefreshToken();
                newRefreshToken.UserId = userInBase.Id;
                var refreshTokenToBase = _mapper.Map<RefreshTokenEntity>(newRefreshToken);
                refreshTokenInBase = _mapper.Map<RefreshToken>(await _refreshTokenRepository.AddRefreshToken(refreshTokenToBase));
            }

            var authResult = new AuthResult()
            {
                Token = jwtToken,
                TokenRefresh = refreshTokenInBase.Token
            };

            _logger.Info($"jwt token for {userInBase} reissued");

            return authResult;
        }

        public async Task<bool> RevokeRefreshToken(string token)
        {
            _logger.Info("The procedure for revoking refresh token has begun");

            var userInBase = _mapper.Map<User>(await _userRepository.GetUserByToken(token));

            if (userInBase == null)
            {
                throw new AuthenticationException("$Authentication failed,user not found");
            }

            if (userInBase.Status != LeadStatusEnum.Active)
            {
                throw new AuthenticationException($"The {userInBase} status does not allow authentication");
            }

            var refreshTokenInBase = userInBase.RefreshTokens.SingleOrDefault(t => t.Token == token);

            if (!refreshTokenInBase!.IsActive)
            {
                throw new RefreshTokenException("Refresh token is not active");
            }

            var result = await _refreshTokenRepository.RevokeToken(token);

            _logger.Info($"The procedure for revoking the refresh token for the {userInBase} is completed");

            return result;
        }

        public async Task<AuthResult> UpdatePassword(User user)
        {
            _logger.Info($"Start the {user} update password user process");

            var userInBase = _mapper.Map<User>(await _userRepository.GetUserByEmailAsync(user.Email));

            if (userInBase != null && await AuthenticationUser(user, userInBase))
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.UpdateUserPassword);
                userInBase.Password = passwordHash;
                var userToBase = _mapper.Map<UserEntity>(userInBase);
                var updateUserEntity = await _userRepository.UpdateUserAsync(userToBase);

                if (updateUserEntity == null)
                {
                    throw new RegistrationException($"Failed to update {userInBase}");
                }

                updateUserEntity.Password = user.UpdateUserPassword;
                var AuthResult = await Authenticate(_mapper.Map<User>(updateUserEntity));


                return AuthResult;
            }
            else
            {
                throw new AuthenticationException($"Authentication failed, invalid email or password. Or user not active");
            }
        }

        private async Task<bool> AuthenticationUser(User userAuth, User userInBase)
        {
            if (await _userRepository.CheckEmailAsync(userAuth.Email))
            {
                if (userInBase.Status != LeadStatusEnum.Active)
                {
                    throw new AuthenticationException($"The {userInBase} status does not allow authentication");
                }

                if (BCrypt.Net.BCrypt.Verify(userAuth.Password, userInBase.Password))
                {
                    return true;
                }
            }
            return false;
        }

        private string CreateTokenJWT(IEnumerable<string> roles, User user)
        {
            var ExpiresRefreshTokenMinut = Convert.ToInt32(_config.GetSection("JwtTokenSettings:ExpiresTokenMinut)").Value!);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.UtcNow.AddMinutes(ExpiresRefreshTokenMinut)).ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.Role,user.Role.ToString()),
                new Claim("Status",user.Status.ToString())

            };

            claims.AddRange(roles.Select(ur => new Claim(ur, "true")));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("PrivateKey")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken CreateRefreshToken()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                };
            }
        }

        private IEnumerable<string> GetRoleAuth(User user)
        {
            if (user.Role == LeadRoleEnum.StandartLead)
            {
                return new[] { LeadRoleEnum.StandartLead.ToString() };
            }
            else if (user.Role == LeadRoleEnum.VipLead)
            {
                return new[] { LeadRoleEnum.StandartLead.ToString(), LeadRoleEnum.VipLead.ToString() };
            }
            else if (user.Role == LeadRoleEnum.Manager)
            {
                return new[] { LeadRoleEnum.Manager.ToString() };
            }

            return null!;
        }
    }
}
