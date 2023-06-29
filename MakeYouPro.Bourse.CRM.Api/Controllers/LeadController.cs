using AutoMapper;
using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Models.Lead.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MakeYouPro.Bourse.CRM.Api.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;

        private readonly IMapper _mapper;

        private readonly IValidator<CreateLeadRequest> _createValidator;

        private readonly IValidator<UpdateLeadUsingLeadRequest> _updateUsingLeadValidator;

        private readonly IValidator<UpdateLeadUsingManagerRequest> _updateUsingManagerValidator;

        public LeadController(ILeadService leadService, IMapper mapper, IValidator<CreateLeadRequest> validator,
                             IValidator<UpdateLeadUsingLeadRequest> updateUsingLeadValidator, IValidator<UpdateLeadUsingManagerRequest> updateUsingManagerValidator)
        {
            _leadService = leadService;
            _mapper = mapper;
            _createValidator = validator;
            _updateUsingLeadValidator = updateUsingLeadValidator;
            _updateUsingManagerValidator = updateUsingManagerValidator;

        }


        [AllowAnonymous]
        [HttpPost(Name = "CreateOrRecoverLeadAsync")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.Conflict)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.Forbidden)]
        [SwaggerResponse((int)HttpStatusCode.PreconditionFailed)]
        public async Task<ActionResult<LeadResponseInfo>> CreateLeadAsync(CreateLeadRequest addLead)
        {
            var validationResult = await _createValidator.ValidateAsync(addLead);

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
            var lead = await _leadService.GetLeadByIdAsync(leadId);
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

        [Authorize(Roles = "StandartLead", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("usingLead", Name = "UpdateLeadUsingLead")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<LeadResponseInfo>> UpdateLeadUsingLeadAsync(UpdateLeadUsingLeadRequest updateRequestLead)
        {
            var validationResult = await _updateUsingLeadValidator.ValidateAsync(updateRequestLead);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var lead = _mapper.Map<Lead>(updateRequestLead);
            var updateLead = await _leadService.UpdateLeadUsingLeadAsync(lead);
            var result = _mapper.Map<LeadResponseInfo>(updateLead);

            return Ok(result);
        }

        [Authorize(Roles = "ManagerLead", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("usingManager", Name = "UpdateLeadUsingManager")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<LeadResponseInfo>> UpdateLeadUsingManagerAsync(UpdateLeadUsingManagerRequest updateRequestLead, int managerId)
        {
            var validationResult = await _updateUsingManagerValidator.ValidateAsync(updateRequestLead);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var lead = _mapper.Map<Lead>(updateRequestLead);
            var updateLead = await _leadService.UpdateLeadUsingManagerAsync(lead, managerId);
            var result = _mapper.Map<LeadResponseInfo>(updateLead);

            return Ok(result);
        }

        [Authorize(Roles = "ManagerLead", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
