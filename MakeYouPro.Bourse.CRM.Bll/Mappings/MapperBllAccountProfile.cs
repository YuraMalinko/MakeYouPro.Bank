using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Bll.Mappings
{
    public class MapperBllAccountProfile : Profile
    {
        public MapperBllAccountProfile()
        {
            CreateMap<Account, AccountEntity>().ReverseMap();
        }
    }
}
