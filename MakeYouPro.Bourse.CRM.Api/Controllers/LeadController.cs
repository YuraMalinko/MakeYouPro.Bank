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

        [HttpPost(Name = "CreateOrRecoverLeadAsync")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.Conflict)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<LeadResponseInfo>> CreateLeadAsync(CreateLeadRequest addLead)
        {
            var leadBll = _mapper.Map<Lead>(addLead);
            var addLeadBll = await _leadService.CreateOrRecoverLeadAsync(leadBll);
            var result = _mapper.Map<LeadResponseInfo>(addLeadBll);

            return Created(new Uri("api/Lead", UriKind.Relative), result);
        }

    }
}
