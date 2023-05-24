using AutoMapper;
using AutoMapper.Configuration.Conventions;
using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bource.CRM.Core.Enums;
using MakeYouPro.Bource.CRM.Dal.Models;

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
