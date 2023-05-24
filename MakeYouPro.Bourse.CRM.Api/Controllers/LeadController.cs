using Microsoft.AspNetCore.Mvc;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using ILogger = NLog.ILogger;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bank.CRM.Bll.Models;

namespace MakeYouPro.Bourse.CRM.Api.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;

        private readonly IMapper _mapper;

        private readonly ILogger _logger;

        public LeadController(ILeadService leadService, IMapper mapper, ILogger nLogger)
        {
            _leadService = leadService;
            _mapper = mapper;
            _logger = nLogger;
        }

        [HttpPost(Name = "CreateLeadAsync")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        public async Task<ActionResult<LeadResponseInfo>> CreateLeadAsync(LeadRequestCreate addLead)
        {
            var leadBll = _mapper.Map<Lead>(addLead);
            var addLeadBll = await _leadService.CreateLeadAsync(leadBll);
            var result = _mapper.Map<LeadResponseInfo>(addLeadBll);

            return Created(new Uri("api/Lead", UriKind.Relative), result);
        }

    }
}
