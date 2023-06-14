using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Repository
{
    public class JpyRepository: IJpyRepository
    {
        private static Context _context;
        public JpyRepository()
        {
            _context = new Context();
        }
        public JPYDto AddJPY(JPYDto jpy)
        {
            _context.MainJPY.Add(jpy);
            _context.SaveChanges();

            return jpy;
        }
    }
}
