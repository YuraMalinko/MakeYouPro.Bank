using AutoMapper;
using MakeYouPro.Bank.Dal.Auth.Models;
using MakeYouPro.Bank.Service.Auth.Models;

namespace MakeYouPro.Bank.Service.Auth
{
    public class MapperBllUserProfile : Profile
    {
        public MapperBllUserProfile()
        {
            CreateMap<UserDal, User>().ReverseMap();
        }
    }
}
