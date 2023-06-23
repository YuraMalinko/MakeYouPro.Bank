using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Clients.TransactionService;
using MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionServiceClient _transactionServiceClient;

        private readonly ILeadService _leadService;

        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;

        public TransactionService(ITransactionServiceClient transactionServiceClient, ILeadService leadService, IAccountService accountService, IMapper mapper)
        {
            _transactionServiceClient = transactionServiceClient;
            _leadService = leadService;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<decimal> GetAccountBalanceAsync(int accountId)
        {
            var balance = await _transactionServiceClient.GetAccountBalanceAsync(accountId);

            return balance;
        }

        public async Task<int> CreateWithdrawAsync(Transaction transaction)
        {
            if (await GetAndCheckAccountBelongsToLeadAsync(transaction.LeadId, transaction.AccountId))
            {
                var account = await _accountService.GetAccountAsync(transaction.AccountId);

                if (account.Currency == "RUB" || account.Currency == "USD")
                {
                    var balance = await _transactionServiceClient.GetAccountBalanceAsync(transaction.AccountId);

                    var commissionPercentage = decimal.Parse(Environment.GetEnvironmentVariable("WithdrawCommissionPercentage"));
                    var amountWithCommission = transaction.Amount + (commissionPercentage * transaction.Amount / 100);
                   // СЮДА ДОПИСАТЬ ПОДПИСКУ

                    if (balance >= amountWithCommission)
                    {
                        transaction.Amount = amountWithCommission;
                        var withdraw = _mapper.Map<WithdrawDtoRequest>(transaction);
                        var transactionId = await _transactionServiceClient.CreateTransactionAsync(withdraw);

                        return transactionId;
                    }
                    else
                    {
                        throw new InsufficientFundsException();
                    }
                }
                else
                {
                    throw new UnsuitableCurrencyException(account.Currency);
                }
            }
            else
            {
                throw new ArgumentException("Lead not found with such an active account");
            }
        }

        private async Task<bool> GetAndCheckAccountBelongsToLeadAsync(int leadId, int accountId)
        {
            var lead = await _leadService.GetLeadByIdAsync(leadId);

            if (lead.Accounts.Any(a => a.Id == accountId && a.Status == AccountStatusEnum.Active))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
