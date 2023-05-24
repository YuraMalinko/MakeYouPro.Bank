using AutoMapper;
using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;

namespace MakeYouPro.Bourse.CRM.Api.Mappings
{
    public class MapperApiLeadProfile : Profile
    {
        public MapperApiLeadProfile()
        {
            CreateMap<CreateLeadRequest, Lead>();
            CreateMap<Lead, LeadResponseInfo>();
        }
    }
}
