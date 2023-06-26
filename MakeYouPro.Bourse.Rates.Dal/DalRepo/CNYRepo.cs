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
    public class CNYRepo : ICnyRepository
    {
        private static Context _context;
        public CNYRepo()
        {
            _context = new Context();
        }
        public CNYDto AddCnyToDb(CNYDto rate)
        {
            _context.MainCNY.Add(rate);
            _context.SaveChanges();

            return rate;
        }
    }
}