using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(): base("There are not enough funds on the account to carry out this operation")
        {
        }
    }
}
