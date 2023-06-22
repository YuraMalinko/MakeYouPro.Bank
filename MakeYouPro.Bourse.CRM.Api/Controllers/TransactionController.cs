using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;
using MakeYouPro.Bourse.CRM.Api.Models.Transaction.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Transaction.Response;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Bll.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MakeYouPro.Bourse.CRM.Api.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost(Name = "GetBalance")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<decimal>> GetBalanceAsync(int accountId)
        {
            var result = await _transactionService.GetAccountBalanceAsync(accountId);

            return Ok(result);
        }

        //[HttpPost(Name = "CreateWithdrawAsync")]
        ////[SwaggerResponse((int)HttpStatusCode.Created)]
        ////[SwaggerResponse((int)HttpStatusCode.Conflict)]
        ////[SwaggerResponse((int)HttpStatusCode.BadRequest)]
        ////[SwaggerResponse((int)HttpStatusCode.NotFound)]
        //public async Task<ActionResult<TransactionResponse>> CreateWithdrawAsync(TransactionRequest transaction)
        //{
        //    //var validationResult = await _validator.ValidateAsync(addLead);

        //    //if (!validationResult.IsValid)
        //    //{
        //    //    return BadRequest(validationResult.Errors);
        //    //}

        //    var leadBll = _mapper.Map<Lead>(addLead);
        //    var addLeadBll = await _leadService.CreateOrRecoverLeadAsync(leadBll);
        //    var result = _mapper.Map<LeadResponseInfo>(addLeadBll);

        //    return Created(new Uri("api/Lead", UriKind.Relative), result);
        //}


    }
}
