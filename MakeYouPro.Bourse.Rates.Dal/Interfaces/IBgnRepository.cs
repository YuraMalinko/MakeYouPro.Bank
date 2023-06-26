using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Interfaces
{
    public interface IBgnRepository
    {
        public BGNDto AddBgnToDb(BGNDto bgn);
    }
}
