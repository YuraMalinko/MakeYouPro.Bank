using AutoMapper;
using MakeYouPro.Bourse.LeadStatusUpdater.Bll;
using MakeYouPro.Bourse.LeadStatusUpdater.Api.Models;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Api
{
    public class MapperApiSettingsProfile : Profile
    {
        public MapperApiSettingsProfile() 
        {
            CreateMap<SettingsRequestDto, Settings>();
            CreateMap<Settings, SettingsResponseDto>();
        }
    }
}
