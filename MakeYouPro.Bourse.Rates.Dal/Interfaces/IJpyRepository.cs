

using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Interfaces
{
    public interface IJpyRepository
    {
        public JPYDto AddJpyToDb(JPYDto rate);

    }
}
