using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;

namespace MakeYouPro.Bourse.Rates.Dal.Repository
{
    public class DateTimeRepository: IDateTimeRepository
    {
        private static Context context;
        public DateTimeRepository()
        {
            context = new Context();
        }
    }
}
