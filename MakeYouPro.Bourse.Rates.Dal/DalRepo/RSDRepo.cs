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
    public class RSDRepo : IRsdRepository
    {
        private static Context _context;
        public RSDRepo()
        {
            _context = new Context();
        }
        public RSDDto AddRsdToDb(RSDDto rate)
        {
            _context.MainRSD.Add(rate);
            _context.SaveChanges();

            return rate;
        }
    }
}

