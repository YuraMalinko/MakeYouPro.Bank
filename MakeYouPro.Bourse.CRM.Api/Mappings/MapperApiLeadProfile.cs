using AutoMapper;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Extensions;
using MakeYouPro.Bourse.CRM.Models.Lead.Response;

namespace MakeYouPro.Bourse.CRM.Api.Mappings
{
    public class MapperApiLeadProfile : Profile
    {
        public MapperApiLeadProfile()
        {
            CreateMap<CreateLeadRequest, Lead>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.FormatEmail()))
                .ForMember(dest => dest.PassportNumber, opt => opt.MapFrom(src => src.PassportNumber.FormatPassportNumber()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.FormatPhoneNumber()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.FormatName()))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName.FormatName()))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname.FormatName()))
                .ForMember(dest => dest.Citizenship, opt => opt.MapFrom(src => src.Citizenship.FormatCitizenship()));
            CreateMap<Lead, LeadResponseInfo>();
            CreateMap<Lead, LeadResponseBase>();
            CreateMap<Lead, LeadResponseMinInfo>();
        }
    }
}
