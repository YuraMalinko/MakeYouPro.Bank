using AutoMapper;
using FluentValidation;
using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bank.CRM.Models.Lead.Response;
using MakeYouPro.Bource.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using ILogger = NLog.ILogger;

namespace MakeYouPro.Bourse.CRM.Api.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;

        private readonly IAuthServiceClient _authServiceClient;

        private readonly IMapper _mapper;

        private readonly IValidator<CreateLeadRequest> _validator;

        private readonly ILogger _logger;

        public LeadController(ILeadService leadService, IAuthServiceClient authServiceClient, IMapper mapper, IValidator<CreateLeadRequest> validator, ILogger nLogger)
        {
            _leadService = leadService;
            _authServiceClient = authServiceClient;
            _mapper = mapper;
            _validator = validator;
            _logger = nLogger;
        }

        [HttpPost(Name = "CreateOrRecoverLeadAsync")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.Conflict)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<LeadResponseInfo>> CreateLeadAsync(CreateLeadRequest addLead)
        {
            var validationResult = await _validator.ValidateAsync(addLead);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var leadBll = _mapper.Map<Lead>(addLead);
            var addLeadBll = await _leadService.CreateOrRecoverLeadAsync(leadBll);
            var result = _mapper.Map<LeadResponseInfo>(addLeadBll);

            return Created(new Uri("api/Lead", UriKind.Relative), result);
        }

        [HttpGet(Name = "GetLeadByIdAsync")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<LeadResponseInfo>> GetLeadById(int leadId)
        {
            var lead = await _leadService.GetLeadById(leadId);
            var result = _mapper.Map<LeadResponseInfo>(lead);

            return Ok(result);
        }

        [HttpDelete(Name = "DeleteLeadByIdAsync")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteLeadByIdAsync(int leadId)
        {
            await _leadService.DeleteLeadByIdAsync(leadId);

            return NoContent();
        }

        [HttpPut("usingLead", Name = "UpdateLeadUsingLead")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<LeadResponseInfo>> UpdateLeadUsingLeadAsync(UpdateLeadUsingLeadRequest updateRequestLead)
        {
            var lead = _mapper.Map<Lead>(updateRequestLead);
            var updateLead = await _leadService.UpdateLeadUsingLeadAsync(lead);
            var result = _mapper.Map<LeadResponseInfo>(updateLead);

            return Ok(result);
        }

        [HttpPut("usingManager", Name = "UpdateLeadUsingManager")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<LeadResponseInfo>> UpdateLeadUsingManagerAsync(UpdateLeadUsingManagerRequest updateRequestLead, int managerId)
        {
            var lead = _mapper.Map<Lead>(updateRequestLead);
            var updateLead = await _leadService.UpdateLeadUsingManagerAsync(lead, managerId);
            var result = _mapper.Map<LeadResponseInfo>(updateLead);

            return Ok(result);
        }

        [HttpPatch("leadRole", Name = "UpdateLeadRoleAsync")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<LeadResponseBase>> UpdateLeadRoleAsync(LeadRoleEnum leadRole, int leadId)
        {
            var lead = await _leadService.UpdateLeadRoleAsync(leadRole, leadId);
            var result = _mapper.Map<LeadResponseBase>(lead);

            return Ok(result);
        }
    }
}
