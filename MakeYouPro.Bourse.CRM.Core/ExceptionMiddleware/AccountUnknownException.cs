using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class AccountUnknownException:Exception
    {
        public AccountUnknownException(string? message) : base($"An unknown error occurred while working with the Account," +
            $" Administrator intervention is required : {message}")
        {
        }
    }
}
