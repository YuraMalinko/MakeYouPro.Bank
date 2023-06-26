using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bourse.Rates.Dal
{
    public class USDRepo : IUsdRepository
    {
        private static Context _context;
        public USDRepo()
        {
            _context = new Context();
        }
        public USDDto AddUsdToDb(USDDto rate)
        {
            _context.MainUSD.Add(rate);
            _context.SaveChanges();

            return rate;
        }
    }
}
