using ReportingService.Dal.IRepository.CRM;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Bll.Services
{
    public class RecordingServices : IRecordingServices
    {
        private readonly ILeadRepository _leadRepositry;
        private readonly IAccountRepository _accountRepositry;

        public RecordingServices(ILeadRepository leadRepositry, IAccountRepository accountRepositry)
        {
            _leadRepositry = leadRepositry;
            _accountRepositry = accountRepositry;
        }

        public async Task CreateLeadInDatabaseAsync(LeadEntity lead)
        {
            await _leadRepositry.CreateLeadAsync(lead);
        }

        public async Task CreateAccountInDatabaseAsync(AccountEntity account)
        {
            await _accountRepositry.CreateAccountAsync(account);
        }
    }
}
