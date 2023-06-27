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
        public ActionResult<List<SettingsResponseDto>> GetSettings()
        {
            var allUsers = _settingsService.GetSettings();
            //var allUsersResponseDto = _mapper.Map<SettingsResponseDto>(allUsers);
            return Ok();
        }
    }
}
