using AutoMapper;
using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bource.CRM.Dal.Models;

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
