using MakeYouPro.Bank.CRM.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Bll.IServices
{
    public interface IAccountService
    {
        Task<Account> CreateAccount(Account account);
    }
}
