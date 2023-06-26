using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal
{
    public class EURRepo : IEurRepository
    {
        private static Context _context;
        public EURRepo()
        {
            _context = new Context();
        }
        public EURDto AddEurToDb(EURDto rate)
        {
            _context.MainEUR.Add(rate);
            _context.SaveChanges();

            return rate;
        }
    }
}

