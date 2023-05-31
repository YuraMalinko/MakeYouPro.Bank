using ReportingService.Dal.IRepository.CRM;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Bll.Services
{
    public class RecordingServices : IRecordingServices
    {
        private readonly ILeadRepository _leadRepositry;

        public RecordingServices(ILeadRepository leadRepositry)
        {
            _leadRepositry = leadRepositry;
        }

        public async Task CreateLeadInDatabaseAsync(LeadEntity lead)
        {
            await _leadRepositry.CreateLeadAsync(lead);
        }

    }
}
