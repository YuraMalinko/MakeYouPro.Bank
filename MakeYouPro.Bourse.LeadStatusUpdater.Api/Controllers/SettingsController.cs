using AutoMapper;
using MakeYouPro.Bourse.LeadStatusUpdater.Api.Models;
using MakeYouPro.Bourse.LeadStatusUpdater.Bll;
using Microsoft.AspNetCore.Mvc;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISettingsService _settingsService;
        public SettingsController(ISettingsService settingsService, IMapper mapper)
        {
            _mapper = mapper;
            _settingsService = settingsService;

        }

        [HttpGet(Name = "GetAllSettings")]
        public ActionResult<SettingsResponseDto> GetSettings()
        {
            var SettingsResponse = _settingsService.GetSettings();
            var SettingsResponseDto = _mapper.Map<SettingsResponseDto>(SettingsResponse);
            return Ok(SettingsResponseDto);
        }

        [HttpPost(Name = "GetAllSettings")]
        public ActionResult<SettingsResponseDto> СhangeSettings(SettingsRequestDto settingsReques)
        {
            var settings = _mapper.Map<Settings>(settingsReques);
            var SettingsResponse = _settingsService.СhangeSettings(settings);
            var SettingsResponseDto = _mapper.Map<SettingsResponseDto>(SettingsResponse);
            return Ok(SettingsResponseDto);
        }
    }
}
