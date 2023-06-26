using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

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