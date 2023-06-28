using AutoMapper;
using MakeYouPro.Bourse.Rates.Bll.ModelsBll;

namespace WebAPIClient
{
    public class MapperApi : Profile
    {
        public MapperApi()
        {
            CreateMap<Data, ARSBll>();
            CreateMap<Data, BGNBll>();
            CreateMap<Data, CNYBll>();
            CreateMap<Data, EURBll>();
            CreateMap<Data, JPYBll>();
            CreateMap<Data, RSDBll>();
            CreateMap<Data, RUBBll>();
            CreateMap<Data, USDBll>();
            CreateMap<ARSBll, Data>();
            CreateMap<BGNBll, Data>();
            CreateMap<CNYBll, Data>();
            CreateMap<EURBll, Data>();
            CreateMap<JPYBll, Data>();
            CreateMap<RSDBll, Data>();
            CreateMap<RUBBll, Data>();
            CreateMap<USDBll, Data>();
        }
    }
}
