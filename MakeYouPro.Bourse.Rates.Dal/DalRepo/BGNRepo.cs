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
    public class BGNRepo : IBgnRepository
    {
        private static Context _context;
        public BGNRepo()
        {
            _context = new Context();
        }
        public BGNDto AddBgnToDb(BGNDto rate)
        {
            _context.MainBGN.Add(rate);
            _context.SaveChanges();

            return rate;
        }
    }
}