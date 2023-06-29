using AutoMapper;
using MakeYouPro.Bourse.CRM.Auth.Bll.Models;
using MakeYouPro.Bourse.CRM.Auth.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Auth.Bll.Mappings
{
    public class MapperBllUserProfile : Profile
    {
        public MapperBllUserProfile()
        {
            CreateMap<UserEntity, User>().ReverseMap();
        }
    }
}
