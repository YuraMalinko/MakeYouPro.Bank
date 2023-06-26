using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal
{
    public class JPYRepo : IJpyRepository
    {
        private static Context _context;
        public JPYRepo()
        {
            _context = new Context();
        }
        public JPYDto AddJpyToDb(JPYDto rate)
        {
            _context.MainJPY.Add(rate);
            _context.SaveChanges();

            return rate;
        }
    }
}

