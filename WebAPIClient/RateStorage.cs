using System;
using System.Text.Json;
using Newtonsoft.Json;

namespace WebAPIClient

{
    public static class RateStorage
    {
        private  static ModelRates _model;
        public static Dictionary <string,decimal> ModifyJsonToDictionaty(string json)
        {
            string[] cuttedJson = json.Split('{', '}');
            //string ratesForM = cuttedJson[2];
            //string ratesForModel = "{"+ratesForM/*.Replace('.', ',')*/+"}";
            //Console.WriteLine(ratesForModel);

            //WriteRatesToModel(ratesForModel);
            string[] preRates = cuttedJson[2].Split('"', ':', ',');
            preRates = preRates.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
            for (int i = 0; i < preRates.Length; i++)
            {
                string key = preRates[i];
                decimal value = Convert.ToDecimal(preRates[++i].Replace('.', ','));
                rates.Add(key, value);
            }
            return rates;
        }
        public static void DeserializeJson (string json)
        {
           string[] cuttedJson = json.Split('{', '}');
            string jsonForDeserialize = "{" + cuttedJson[2] + "}";
            Console.WriteLine(jsonForDeserialize);
            var model = JsonConvert.DeserializeObject<ModelRates>(jsonForDeserialize);
            model.DateTime = DateTime.Now;
            Console.WriteLine(model.BGNRUB);
            Console.WriteLine(model.DateTime);

        }

    }
}
