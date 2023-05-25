using ReportingService.Bll.Models.CRM;
using ReportingService.Dal.IRepository.CRM;

namespace ReportingService.Bll.Services
{
    public class RecordingServices
    {
        private readonly ILeadRepository leadRepositry;

        public RecordingServices() 
        {

        }

        public void CreateAnEntryInDatabase<T>(T record)where T : class
        {
            if(record is Lead)
            {

            }
            else if(record is Account) 
            {

            }
        }

    }
}
