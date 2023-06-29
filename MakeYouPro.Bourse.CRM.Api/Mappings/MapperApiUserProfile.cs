using AutoMapper;
using MakeYouPro.Bourse.CRM.Api.Models.Users.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Users.Response;
using MakeYouPro.Bourse.CRM.Auth.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Extensions;

namespace MakeYouPro.Bourse.CRM.Api.Mappings
{
    public class MapperApiUserProfile:Profile
    {
        public MapperApiUserProfile()
        {
            CreateMap<UserBaseRequest, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.FormatEmail()));
            CreateMap<UserUpdateRequest, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.FormatEmail()));
            CreateMap<AuthResult,AuthResultResponse>();
        }
    }
}
