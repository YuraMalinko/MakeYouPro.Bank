﻿using MakeYouPro.Bank.Dal.Auth.Models;
using MakeYouPro.Bank.Service.Auth.Models;

namespace MakeYouPro.Bank.Service.Auth.Services
{
    public interface IAuthService
    {
        Task<User> RegisterUserAsync(User user);

        //Task<bool> SendMessageAsync(Message message);

        Task<string> GetUserByEmail(User user);
    }
}