using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal
{
    public class BGNRepo : IBgnRepository
    {
        private static Context _context;
        public BGNRepo()
        {
            _context = new Context();
        }
        public BGNDto AddBgnToDb(BGNDto rate)
        {
            _context.MainBGN.Add(rate);
            _context.SaveChanges();

            return rate;
        }
    }
}