using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Interfaces
{
    public interface IUsdRepository
    {
        public USDDto AddUsdToDb(USDDto rate);
    }
}
