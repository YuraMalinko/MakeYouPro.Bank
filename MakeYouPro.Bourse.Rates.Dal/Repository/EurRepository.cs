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
    public class EurRepository: IEurRepository
    {
        private static Context _context;
        public EurRepository()
        {
            _context = new Context();
        }
        public EURDto AddEur(EURDto eur)
        {
            _context.MainEUR.Add(eur);
            _context.SaveChanges();

            return eur;
        }
    }
}
