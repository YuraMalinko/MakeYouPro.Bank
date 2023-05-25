using ReportingService.Dal.IRepository.CRM;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Bll.Services
{
    public class RecordingServices
    {
        private readonly ILeadRepository _leadRepositry;

        public RecordingServices(ILeadRepository leadRepositry)
        {
            _leadRepositry = leadRepositry;
        }

        public async Task CreateAnEntryInDatabaseAsync<T>(T record)
        {
            var type = record.GetType();
            if (type == typeof(LeadEntity))
            {
                await _leadRepositry.CreateLeadAsync(record as LeadEntity);
            }
            else if (type == typeof(AccountEntity))
            {

            }
            else 
            { 
                throw new ArgumentException("Unnknow record type"); 
            }
        }

    }
}
