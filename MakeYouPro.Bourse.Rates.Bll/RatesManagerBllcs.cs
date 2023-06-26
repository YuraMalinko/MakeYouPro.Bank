using MakeYouPro.Bourse.Rates.Dal.Models;
using MakeYouPro.Bourse.Rates.Dal;
using MakeYouPro.Bourse.Rates.Bll;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;
using MakeYouPro.Bourse.Rates.Dal.Models;
using MakeYouPro.Bourse.Rates.Dal;
using MakeYouPro.Bourse.Rates.Bll.ModelsBll;

namespace MakeYouPro.Bourse.Rates.Bll
{
    public class RatesManagerBll
    {
        private MapperBll _instanceMapperBll = MapperBll.getInstance();
        private readonly IArsRepository _arsRepository;
        private readonly IBgnRepository _bgnRepository;
        private readonly ICnyRepository _cnyRepository;
        private readonly IEurRepository _eurRepository;
        private readonly IJpyRepository _jpyRepository;
        private readonly IRsdRepository _rsdRepository;
        private readonly IRubRepository _rubRepository;
        private readonly IUsdRepository _usdRepository;

        public RatesManagerBll()
        {
            _arsRepository = new ARSRepo();
           _bgnRepository = new BGNRepo();
           _cnyRepository = new CNYRepo();
           _eurRepository = new EURRepo();
           _jpyRepository = new JPYRepo();
           _rsdRepository = new RSDRepo();
           _rubRepository = new RUBRepo();
           _usdRepository = new USDRepo();
        }


        public void SaveARSRatesToDB(ARSBll arsBll)
        {

        }
    }
}
