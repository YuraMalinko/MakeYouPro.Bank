using AutoMapper;
using MakeYouPro.Bank.Api.Auth.Models.Requests;
using MakeYouPro.Bank.Service.Auth.Models;

namespace MakeYouPro.Bank.Api.Auth
{
    public class MapperApiUserProfile : Profile
    {
        public MapperApiUserProfile()
        {
            CreateMap<UserRegisterRequest, User>();
        }
    }
}
