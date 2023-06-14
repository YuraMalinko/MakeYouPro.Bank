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
    public class ArsRepository: IArsRepository
    {
        private static Context _context;
        public ArsRepository()
        {
            _context = new Context();
        }
        public ARSDto AddArs(ARSDto ars)
        {
            _context.MainARS.Add(ars);
            _context.SaveChanges();

            return ars;
        }
    }
}
