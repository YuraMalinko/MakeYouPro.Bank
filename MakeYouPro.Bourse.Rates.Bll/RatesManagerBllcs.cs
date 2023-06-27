using MakeYouPro.Bourse.Rates.Dal.Models;
using MakeYouPro.Bourse.Rates.Dal;
using MakeYouPro.Bourse.Rates.Dal.Interfaces;
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

        public void SaveARSRatesToDB(ARSBll rate)
        {
            Console.WriteLine("suc");
            _arsRepository.AddArsToDb(_instanceMapperBll.MapARSBllToARSDto(rate));
        }
        public void SaveBGNRatesToDB (BGNBll rate)
        {
            _bgnRepository.AddBgnToDb(_instanceMapperBll.MapBGNBllToBGNDto(rate));
        }
        public void SaveCNYRatesToDB (CNYBll rate)
        {
            _cnyRepository.AddCnyToDb(_instanceMapperBll.MapCNYBllToCNYDto(rate));
        }
        public void SaveEURRatesToDB(EURBll rate)
        {
            _eurRepository.AddEurToDb(_instanceMapperBll.MapEURBllToEURDto(rate));
        }
        public void SaveJPYRatesToDB(JPYBll rate)
        {
            _jpyRepository.AddJpyToDb(_instanceMapperBll.MapJPYBllToJPYDto(rate));
        }
        public void SaveRSDRatesToDB(RSDBll rate)
        {
            _rsdRepository.AddRsdToDb(_instanceMapperBll.MapRSDBllToRSDDto(rate)); 
        }
        public void SaveRUBRatesToDB(RUBBll rate)
        {
            _rubRepository.AddRubToDb(_instanceMapperBll.MapRUBBllToRUBDto(rate));
        }
        public void SaveUSDRatesToDB(USDBll rate)
        {
            _usdRepository.AddUsdToDb(_instanceMapperBll.MapUSDBllToUSDDto(rate));
        }
    }
}
