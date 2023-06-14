using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Repository
{
    public class UsdRepository: IUsdRepository
    {
        private static Context _context;
        public UsdRepository()
        {
            _context = new Context();
        }
        public USDDto AddUsd(USDDto usd)
        {
            _context.MainUSD.Add(usd);
            _context.SaveChanges();

            return usd;
        }
    }
}
