using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal
{
    public class ARSRepo: IArsRepository
    {
        private static Context _context;
        public ARSRepo()
        {
            _context = new Context();
        }
        public ARSDto AddArsToDb(ARSDto ars)
        {

            _context.MainARS.Add(ars);
            _context.SaveChanges();

            return ars;
        }
    }
}
