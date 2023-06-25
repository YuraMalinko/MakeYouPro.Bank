using AutoMapper;
using MakeYouPro.Bourse.Rates.Bll.ModelsBll;

namespace WebAPIClient
{
    public class MapperApi : Profile
    {
        public MapperApi()
        {
            CreateMap<ModelRates, ARSBll>();
            CreateMap<ModelRates, BGNBll>();
            CreateMap<ModelRates, CNYBll>();
            CreateMap<ModelRates, EURBll>();
            CreateMap<ModelRates, JPYBll>();
            CreateMap<ModelRates, RSDBll>();
            CreateMap<ModelRates, RUBBll>();
            CreateMap<ModelRates, USDBll>();
            CreateMap<ARSBll, ModelRates>();
            CreateMap<BGNBll, ModelRates>();
            CreateMap<CNYBll, ModelRates>();
            CreateMap<EURBll, ModelRates>();
            CreateMap<JPYBll, ModelRates>();
            CreateMap<RSDBll, ModelRates>();
            CreateMap<RUBBll, ModelRates>();
            CreateMap<USDBll, ModelRates>();
        }
    }
}
