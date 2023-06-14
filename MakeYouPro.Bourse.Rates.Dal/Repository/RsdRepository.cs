using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Repository
{
    public class RsdRepository: IRsdRepository
    {
        private static Context _context;
        public RsdRepository()
        {
            _context = new Context();
        }
        public RSDDto AddRsd(RSDDto rsd)
        {
            _context.MainRSD.Add(rsd);
            _context.SaveChanges();

            return rsd;
        }
    }
}
