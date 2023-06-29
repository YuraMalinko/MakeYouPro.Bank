using System.Reflection;
using Newtonsoft.Json;

namespace WebAPIClient
{
    public static class RateStorage
    {
        static bool freshInfo = false;
        public static Dictionary<string, decimal> rateDictionary {get;set;}

        public async static Task GetAndSaveRates()
        {
                using (var client = new HttpClient())
                {
                    Console.WriteLine("Обращение к сервису");
                    var json = await client.GetStringAsync
                ("https://currate.ru/api/?get=rates&pairs=RUBUSD,USDRUB,RUBEUR,EURRUB,RUBJPY,JPYRUB,RUBCNY,CNYRUB,RUBRSD,RSDRUB,RUBBGN,BGNRUB,RUBARS,ARSRUB,USDEUR,EURUSD,USDJPY,JPYUSD,USDCNY,CNYUSD,USDRSD,RSDUSD,USDBGN,BGNUSD,USDARS,ARSUSD,EURJPY,JPYEUR,EURCNY,CNYEUR,EURRSD,RSDEUR,EURBGN,BGNEUR,EURARS,ARSEUR,JPYCNY,CNYJPY,JPYRSD,RSDJPY,JPYBGN,BGNJPY,JPYARS,ARSJPY,CNYRSD,RSDCNY,CNYBGN,BGNCNY,CNYARS,ARSCNY,RSDBGN,BGNRSD,RSDARS,ARSRSD,BGNARS,ARSBGN&key=d93cb3151e643d1cbfe60829b8977980");
             
                var model = RateStorage.DeserializeJson(json);
                    var rateDictionary = RateStorage.ConvertClassToDictionary(model);
                    model.data.DateTime = DateTime.Now;
                    freshInfo = true;
                    Console.WriteLine("Успешно");
                }
        }

        public static bool MarkRatesAsExpires()
        {
            freshInfo = false;
            return freshInfo;
        }

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
                ratesDictionary.Add(info.Name, Convert.ToDecimal(info.GetValue(model.data, null)));
            }
            TimedBackgroundService final = new TimedBackgroundService();

            return ratesDictionary;
        }

    }
}

