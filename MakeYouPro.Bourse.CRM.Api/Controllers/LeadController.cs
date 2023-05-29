using AutoMapper;
using FluentValidation;
using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;
using MakeYouPro.Bourse.CRM.Bll.IServices;
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

        private readonly IMapper _mapper;

        private readonly IValidator<CreateLeadRequest> _validator;

        private readonly ILogger _logger;

        public LeadController(ILeadService leadService, IMapper mapper, IValidator<CreateLeadRequest> validator, ILogger nLogger)
        {
            _leadService = leadService;
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
    }
}
