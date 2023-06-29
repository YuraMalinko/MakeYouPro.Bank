using AutoMapper;
using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Transaction.Request;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
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

        private readonly IValidator<TransactionRequest> _validator;

        private readonly IValidator<TransferTransactionRequest> _transferValidator;

        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IValidator<TransactionRequest> validator,
                                     IValidator<TransferTransactionRequest> transferValidator, IMapper mapper)
        {
            _transactionService = transactionService;
            _validator = validator;
            _transferValidator = transferValidator;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetBalance")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<decimal>> GetBalanceAsync(int accountId)
        {
            var result = await _transactionService.GetAccountBalanceAsync(accountId);

            return Ok(result);
        }

        [HttpPost("Withdraw", Name = "CreateWithdrawAsync")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<int>> CreateWithdrawAsync(TransactionRequest transactionRequest)
        {
            var validationResult = await _validator.ValidateAsync(transactionRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var transaction = _mapper.Map<Transaction>(transactionRequest);

            var result = await _transactionService.CreateWithdrawAsync(transaction);

            return Ok(result);
        }

        [HttpPost("Deposit", Name = "CreateDepositAsync")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<int>> CreateDepositAsync(TransactionRequest transactionRequest)
        {
            var validationResult = await _validator.ValidateAsync(transactionRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var transaction = _mapper.Map<Transaction>(transactionRequest);

            var result = await _transactionService.CreateDepositAsync(transaction);

            return Ok(result);
        }

        [HttpPost("TransferTransaction", Name = "CreateTransferTransactionAsync")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<List<int>>> CreateTransferTransactionAsync(TransferTransactionRequest transactionRequest)
        {
            var validationResult = await _transferValidator.ValidateAsync(transactionRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var transaction = _mapper.Map<TransferTransaction>(transactionRequest);

            var result = await _transactionService.CreateTransferTransactionAsync(transaction);

            return Ok(result);
        }

        //[HttpGet("com", Name = "GetCom")]
        //public void Get()
        //{
        //    var producer = new Produser<CommissionMessage>("localhost", "ex", "que");
        //    var mes = new CommissionMessage
        //    {
        //        CommissionAmount = 100,
        //        TransactionId = 1
        //    };
        //    producer.Publish(mes);
        //}
    }
}
