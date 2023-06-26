using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeYouPro.Bourse.Rates.Dal.Models;

namespace MakeYouPro.Bourse.Rates.Dal.Interfaces
{
    public interface IRsdRepository
    {
        public RSDDto AddRsdToDb(RSDDto rate);

    }
}
