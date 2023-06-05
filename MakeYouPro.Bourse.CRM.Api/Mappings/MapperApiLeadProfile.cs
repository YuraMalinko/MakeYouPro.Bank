using AutoMapper;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Models.Lead.Response;

namespace MakeYouPro.Bourse.CRM.Api.Mappings
{
    public class MapperApiLeadProfile : Profile
    {
        public MapperApiLeadProfile()
        {
            CreateMap<CreateLeadRequest, Lead>();
            CreateMap<Lead, LeadResponseInfo>();
            CreateMap<Lead, LeadResponseBase>();
            CreateMap<Lead, LeadResponseMinInfo>();
        }
    }
}
