using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Clients.TransactionService;
using MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models;
using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionServiceClient _transactionServiceClient;

        private readonly ILeadService _leadService;

        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;

        private readonly ICommissionSettings _commissionSettings;

        public TransactionService(ITransactionServiceClient transactionServiceClient, ILeadService leadService,
                                  IAccountService accountService, IMapper mapper, ICommissionSettings commissionSettings)
        {
            _transactionServiceClient = transactionServiceClient;
            _leadService = leadService;
            _accountService = accountService;
            _mapper = mapper;
            _commissionSettings = commissionSettings;
        }

        public async Task<decimal> GetAccountBalanceAsync(int accountId)
        {
            var balance = await _transactionServiceClient.GetAccountBalanceAsync(accountId);

            return balance;
        }

        public async Task<int> CreateWithdrawAsync(Transaction transaction)
        {
            await GetOrThrowAccountBelongsToLeadAsync(transaction.LeadId, new List<int> { transaction.AccountId });
            var account = await _accountService.GetAccountAsync(transaction.AccountId);

            if (account.Currency == "RUB" || account.Currency == "USD")
            {
                var balance = await _transactionServiceClient.GetAccountBalanceAsync(transaction.AccountId);
                var amountWithCommission = transaction.Amount + (_commissionSettings.WithdrawCommissionPercentage * transaction.Amount / 100);
                // СЮДА ДОПИСАТЬ ПОДПИСКУ

                if (balance >= amountWithCommission)
                {
                    transaction.Amount = amountWithCommission;
                    var withdraw = _mapper.Map<WithdrawRequest>(transaction);
                    var transactionId = await _transactionServiceClient.CreateWithdrawTransactionAsync(withdraw);

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

        public async Task<int> CreateDepositAsync(Transaction transaction)
        {
            await GetOrThrowAccountBelongsToLeadAsync(transaction.LeadId, new List<int> { transaction.AccountId });
            var account = await _accountService.GetAccountAsync(transaction.AccountId);

            if (account.Currency == "RUB" || account.Currency == "USD")
            {
                var amountWithCommission = transaction.Amount + (_commissionSettings.DepositCommissionPercentage * transaction.Amount / 100);
                // СЮДА ДОПИСАТЬ ПОДПИСКУ

                transaction.Amount = amountWithCommission;
                var deposit = _mapper.Map<DepositRequest>(transaction);
                var transactionId = await _transactionServiceClient.CreateDepositTransactionAsync(deposit);

                return transactionId;
            }
            else
            {
                throw new UnsuitableCurrencyException(account.Currency);
            }
        }

        public async Task<List<int>> CreateTransferTransactionAsync(TransferTransaction transaction)
        {
            var lead = await GetOrThrowAccountBelongsToLeadAsync(transaction.AccountSource.LeadId,
                                                                new List<int> { transaction.AccountSource.AccountId, transaction.AccountDestination.AccountId });
            var balance = await _transactionServiceClient.GetAccountBalanceAsync(transaction.AccountSource.AccountId);

            if (lead.Role != LeadRoleEnum.VipLead && lead.Role != LeadRoleEnum.Manager)
            {
                if (transaction.AccountDestination.Currency == "RUB")
                {
                    var amountWithCommission = transaction.Amount + (_commissionSettings.ExtraTransferTransactionCommissionPercentage * transaction.Amount / 100);
                    // СЮДА ДОПИСАТЬ ПОДПИСКУ

                    return await CheckBalanceAndCreateTransferTransactionOrThrowAsync(transaction, balance, amountWithCommission);
                }
                else
                {
                    throw new ArgumentException("As long as you do not receive the status of a VipLid, you can carry out a transfer transaction only to a RUB Account");
                }
            }
            else if (lead.Role == LeadRoleEnum.VipLead)
            {
                var amountWithCommission = transaction.Amount + (_commissionSettings.TransferTransactionCommissionPercentage * transaction.Amount / 100);
                // СЮДА ДОПИСАТЬ ПОДПИСКУ

                return await CheckBalanceAndCreateTransferTransactionOrThrowAsync(transaction, balance, amountWithCommission);
            }

            throw new ArgumentException("Lead does not have a suitable role");
        }

        private async Task<Lead> GetOrThrowAccountBelongsToLeadAsync(int leadId, List<int> accountsId)
        {
            var lead = await _leadService.GetLeadByIdAsync(leadId);

            foreach (var accountId in accountsId)
            {
                if (!lead.Accounts.Any(a => a.Id == accountId && a.Status == AccountStatusEnum.Active))
                {
                    throw new ArgumentException("Lead doesn't have such an active Account");
                }
            }

            return lead;
        }

        private async Task<List<int>> CheckBalanceAndCreateTransferTransactionOrThrowAsync(TransferTransaction transaction, decimal balance, decimal amountWithCommission)
        {
            if (balance >= amountWithCommission)
            {
                transaction.Amount = amountWithCommission;
                var transferTransaction = _mapper.Map<TransferRequest>(transaction);
                var transactionsId = await _transactionServiceClient.CreateTransferTransactionAsync(transferTransaction);

                return transactionsId;
            }
            else
            {
                throw new InsufficientFundsException();
            }
        }
    }
}
