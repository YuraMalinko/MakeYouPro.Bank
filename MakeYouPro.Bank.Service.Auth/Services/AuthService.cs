using AutoMapper;
using MailKit.Net.Smtp;
using MakeYouPro.Bank.Dal.Auth.Models;
using MakeYouPro.Bank.Dal.Auth.Repository;
using MakeYouPro.Bank.Service.Auth.Models;
using MimeKit;

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

                var message = new Message()
                {
                    Email = user.Email,
                    Subject = "Регистрация",
                    Text = "вы зарегистрировались"
                };

                await SendMessageAsync(message);

                return result;
            }
            else
            {
                throw new Exception($"Что-то пошло не так");
            }
        }

        public async Task<bool> SendMessageAsync(Message message)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", Environment.GetEnvironmentVariable("Bank.Email")));
            emailMessage.To.Add(new MailboxAddress("", message.Email));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Text
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 25, false);
                await client.AuthenticateAsync(Environment.GetEnvironmentVariable("Bank.Email"), Environment.GetEnvironmentVariable("Bank.Email.AppPassword"));
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);

                return true;
            }

        }
    }
}
