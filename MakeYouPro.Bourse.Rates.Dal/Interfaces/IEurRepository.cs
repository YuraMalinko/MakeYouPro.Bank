using MakeYouPro.Bourse.Rates.Dal.Models;
namespace MakeYouPro.Bourse.Rates.Dal.Interfaces
{
    public interface IEurRepository
    {
        public EURDto AddEurToDb(EURDto rate);
    }
}
