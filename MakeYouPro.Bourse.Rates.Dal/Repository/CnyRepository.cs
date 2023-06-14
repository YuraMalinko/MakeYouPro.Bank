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
    public class CnyRepository: ICnyRepository
    {
        private static Context _context;
        public CnyRepository()
        {
            _context = new Context();
        }
        public CNYDto AddCny(CNYDto cny)
        {
            _context.MainCNY.Add(cny);
            _context.SaveChanges();

            return cny;
        }
    }
}
