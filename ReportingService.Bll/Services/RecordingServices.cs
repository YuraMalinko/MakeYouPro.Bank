using ReportingService.Bll.Models.CRM;

namespace ReportingService.Bll.Services
{
    public class RecordingServices
    {
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
