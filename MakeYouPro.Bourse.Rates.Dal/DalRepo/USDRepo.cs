using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal
{
    public class USDRepo : IUsdRepository
    {
        private static Context _context;
        public USDRepo()
        {
            _context = new Context();
        }
        public USDDto AddUsdToDb(USDDto rate)
        {
            _context.MainUSD.Add(rate);
            _context.SaveChanges();

            return rate;
        }
    }
}
