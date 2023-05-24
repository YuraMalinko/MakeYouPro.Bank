using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class AccountService : IAccountService
    {
        public async Task<Account> CreateAccount(Account account)
        {
            return new Account();
        }
    }
}
