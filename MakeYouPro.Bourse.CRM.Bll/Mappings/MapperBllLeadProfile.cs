using AutoMapper;
using AutoMapper.Configuration.Conventions;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Bll.Mappings
{
    public class MapperBllLeadProfile : Profile
    {
        public MapperBllLeadProfile() 
        {
            CreateMap<Lead, LeadEntity>();
            CreateMap<LeadEntity, Lead>();
        }
    }
}
