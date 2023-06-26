using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Interfaces
{
    public interface IRsdRepository
    {
        public RSDDto AddRsdToDb(RSDDto rate);
    }
}
