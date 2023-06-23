using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models
{
    public class DepositRequest
    {
        public int AccountId { get; set; }

        public decimal Amount { get; set; }
    }
}
