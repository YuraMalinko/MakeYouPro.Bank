using AutoMapper;
using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MakeYouPro.Bourse.CRM.Api.Controllers
{
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

        [HttpPost(Name = "CreateOrRecoverLeadAsync")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.Conflict)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
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
    }
}
