using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Interfaces
{
    public interface IRubRepository
    {
        public RUBDto AddRubToDb(RUBDto rate);
    }
}
