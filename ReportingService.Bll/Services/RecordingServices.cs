using ReportingService.Bll.IServices;
using ReportingService.Dal.IRepository;
using ReportingService.Dal.IRepository.CRM;
using ReportingService.Dal.Models.CRM;
using ReportingService.Dal.Models.TransactionStore;

namespace ReportingService.Bll.Services
{
    public class RecordingServices : IRecordingServices
    {
        private readonly ILeadRepository _leadRepositry;
        private readonly IAccountRepository _accountRepositry;
        private readonly ITransactionRepository _transactionRepositry;

        public RecordingServices(ILeadRepository leadRepositry,
                                 IAccountRepository accountRepositry,
                                 ITransactionRepository transactionRepository)
        {
            _leadRepositry = leadRepositry;
            _accountRepositry = accountRepositry;
            _transactionRepositry = transactionRepository;
        }

        public async Task CreateLeadInDatabaseAsync(LeadEntity lead)
        {
            await _leadRepositry.CreateLeadAsync(lead);
        }

        public async Task CreateAccountInDatabaseAsync(AccountEntity account)
        {
            await _accountRepositry.CreateAccountAsync(account);
        }

        public async Task UpdateLeadInDatebaseAync(LeadEntity lead)
        {
            await _leadRepositry.UpdateLeadAsync(lead);
        }
        public async Task UpdateAccountInDatebaseAync(AccountEntity account)
        {
            await _accountRepositry.UpdateAccountAsync(account);
        }

        public async Task CreateTransactionAsync(TransactionEntity transaction)
        {
            await _transactionRepositry.CreateTransactionAsync(transaction);
        }

        public async Task UpdateTransactionAsync(TransactionEntity transaction)
        {
            await _transactionRepositry.UpdateTransactionAsync(transaction);
        }
    }
}
