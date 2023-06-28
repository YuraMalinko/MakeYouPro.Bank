using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Interfaces;
using Newtonsoft.Json;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Serializer
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
