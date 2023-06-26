using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal
{
    public class RUBRepo : IRubRepository
    {
        private static Context _context;
        public RUBRepo()
        {
            _context = new Context();
        }
        public RUBDto AddRubToDb(RUBDto rate)
        {
            _context.MainRUB.Add(rate);
            _context.SaveChanges();

            return rate;
        }
    }
}

