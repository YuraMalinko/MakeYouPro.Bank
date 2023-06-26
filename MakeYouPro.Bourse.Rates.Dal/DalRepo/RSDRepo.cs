using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal
{
    public class RSDRepo : IRsdRepository
    {
        private static Context _context;
        public RSDRepo()
        {
            _context = new Context();
        }
        public RSDDto AddRsdToDb(RSDDto rate)
        {
            _context.MainRSD.Add(rate);
            _context.SaveChanges();

            return rate;
        }
    }
}

