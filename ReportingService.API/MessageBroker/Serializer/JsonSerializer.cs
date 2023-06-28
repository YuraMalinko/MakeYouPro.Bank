using Newtonsoft.Json;
using ReportingService.Api.MessageBroker.Interfaces;

namespace ReportingService.Api.MessageBroker.Serializer
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
