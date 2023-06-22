using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Bll.IServices
{
    public interface ITransactionService
    {
        Task<decimal> GetAccountBalanceAsync(int accountId);
    }
}
