using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Core.Configurations.ISettings
{
    public interface ICurrencySetting
    {
         List<string> CurrencyStandart { get; set; }

         List<string> CurrencyVip { get; set; }
    }
}
