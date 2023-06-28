using System.Reflection;
using Newtonsoft.Json;

namespace WebAPIClient
{
    public static class RateStorage
    {
        public static Rootobject DeserializeJson(string json)
        {
            var model = JsonConvert.DeserializeObject<Rootobject>(json);
            Console.WriteLine(model.data.EURBGN);
            return model;
        }

        public static Dictionary<string, decimal> ConvertClassToDictionary(Rootobject model)
        { 
        Dictionary<string, decimal> ratesDictionary = new Dictionary<string, decimal>();

        PropertyInfo[] infos = model.data.GetType().GetProperties();
             
            foreach (PropertyInfo info in infos)
            {
                ratesDictionary.Add(info.Name, Convert.ToDecimal(info.GetValue(model.data, null)) ); 
            }
            return ratesDictionary;
        }
    }
}
