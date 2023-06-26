using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Interfaces
{
    public interface IArsRepository
    { 
        public ARSDto AddArsToDb(ARSDto ars);
    }
}
