using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bourse.Rates.Dal.Repository
{
    public class BgnRepository: IBgnRepository
    {
        private static Context _context;
        public BgnRepository()
        {
            _context = new Context();
        }
        public BGNDto AddBgn(BGNDto bgn)
        {
            _context.MainBGN.Add(bgn);
            _context.SaveChanges();

            return bgn;
        }
    }
}
