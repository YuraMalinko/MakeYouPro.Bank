using MakeYouPro.Bourse.Rates.Bll.ModelsBll;

namespace MakeYouPro.Bourse.Rates.Bll
{
    public interface IRatesBllRepo
    {
        public void SaveARSRatesToDB(ARSBll rate);
        public void SaveBGNRatesToDB(BGNBll rate);
        public void SaveCNYRatesToDB(CNYBll rate);
        public void SaveEURRatesToDB(EURBll rate);
        public void SaveJPYRatesToDB(JPYBll rate);
        public void SaveRSDRatesToDB(RSDBll rate);
        public void SaveRUBRatesToDB(RUBBll rate);
        public void SaveUSDRatesToDB(USDBll rate);
    }
}
