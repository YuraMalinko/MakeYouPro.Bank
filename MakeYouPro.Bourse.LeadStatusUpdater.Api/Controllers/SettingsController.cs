using AutoMapper;
using MakeYouPro.Bourse.LeadStatusUpdater.Api.Models;
using MakeYouPro.Bourse.LeadStatusUpdater.Bll;
using MakeYouPro.Bourse.LeadStatusUpdater.Service;
using Microsoft.AspNetCore.Mvc;
using ILogger = NLog.ILogger;
using LogLevel = NLog.LogLevel;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Api.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISettingsService _settingsService;
        private readonly ILogger _logger;
        public SettingsController(ISettingsService settingsService, IMapper mapper, ILogger nLogger)
        {
            _mapper = mapper;
            _settingsService = settingsService;
            _logger = nLogger;

        }

        [HttpGet(Name = "GetAllSettings")]
        public ActionResult<SettingsResponseDto> GetSettings()
        {
            _logger.Log(LogLevel.Info, $"{nameof(SettingsController)}: Starting {nameof(GetSettings)}");

            var SettingsResponse = _settingsService.GetSettings();
            var SettingsResponseDto = _mapper.Map<SettingsResponseDto>(SettingsResponse);
            return Ok(SettingsResponseDto);
        }

        [HttpPost(Name = "СhangeSettings")]
        public ActionResult<SettingsResponseDto> СhangeSettings(SettingsRequestDto settingsReques)
        {
            _logger.Log(LogLevel.Info, $"{nameof(SettingsController)}: Starting {nameof(СhangeSettings)}");

            var settings = _mapper.Map<Bll.Settings>(settingsReques);
            var SettingsResponse = _settingsService.СhangeSettings(settings);
            var SettingsResponseDto = _mapper.Map<SettingsResponseDto>(SettingsResponse);
            return Ok(SettingsResponseDto);
        }
    }
}
