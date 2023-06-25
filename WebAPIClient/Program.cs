using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using WebAPIClient;
using System.Text.Json;
using Newtonsoft.Json;
using System.Reflection;

public class Program
{

    public static async Task Main(string[] args)
    {   
        HttpClient client = new HttpClient();
        await ProcessRepositoriesAsync(client);
        static async Task ProcessRepositoriesAsync(HttpClient client)
        {
            var json = await client.GetStringAsync("https://currate.ru/api/?get=rates&pairs=RUBUSD,USDRUB,RUBEUR,EURRUB,RUBJPY,JPYRUB,RUBCNY,CNYRUB,RUBRSD,RSDRUB,RUBBGN,BGNRUB,RUBARS,ARSRUB,USDEUR,EURUSD,USDJPY,JPYUSD,USDCNY,CNYUSD,USDRSD,RSDUSD,USDBGN,BGNUSD,USDARS,ARSUSD,EURJPY,JPYEUR,EURCNY,CNYEUR,EURRSD,RSDEUR,EURBGN,BGNEUR,EURARS,ARSEUR,JPYCNY,CNYJPY,JPYRSD,RSDJPY,JPYBGN,BGNJPY,JPYARS,ARSJPY,CNYRSD,RSDCNY,CNYBGN,BGNCNY,CNYARS,ARSCNY,RSDBGN,BGNRSD,RSDARS,ARSRSD,BGNARS,ARSBGN&key=d93cb3151e643d1cbfe60829b8977980");
            RateStorage.DeserializeJson(json);
            Dictionary <string, decimal> s = RateStorage.ModifyJsonToDictionaty(json);
        }
    }
}
            //var json = await client.GetStreamAsync("https://currate.ru/api/?get=rates&pairs=RUBUSD,USDRUB&key=d93cb3151e643d1cbfe60829b8977980");
//Dictionary <string, decimal> rates = RateStorage.ModifyJson(json);

//client.DefaultRequestHeaders.Accept.Clear();


//    //    Console.WriteLine(json);



//}
//Console.Wri65te(rates);
//var json = await client.GetStreamAsync("https://currate.ru/api/?get=rates&pairs=RUBUSD,USDRUB,RUBEUR,EURRUB,RUBJPY,JPYRUB,RUBCNY,CNYRUB,RUBRSD,RSDRUB,RUBBGN,BGNRUB,RUBARS,ARSRUB,USDEUR,EURUSD,USDJPY,JPYUSD,USDCNY,CNYUSD,USDRSD,RSDUSD,USDBGN,BGNUSD,USDARS,ARSUSD,EURJPY,JPYEUR,EURCNY,CNYEUR,EURRSD,RSDEUR,EURBGN,BGNEUR,EURARS,ARSEUR,JPYCNY,CNYJPY,JPYRSD,RSDJPY,JPYBGN,BGNJPY,JPYARS,ARSJPY,CNYRSD,RSDCNY,CNYBGN,BGNCNY,CNYARS,ARSCNY,RSDBGN,BGNRSD,RSDARS,ARSRSD,BGNARS,ARSBGN&key=d93cb3151e643d1cbfe60829b8977980");







////Console.WriteLine(json);
//Console.WriteLine(cuttedJson[2]);
//string[] preRates = cuttedJson[2].Split('"', ':', ',');
//preRates = preRates.Where(x => !string.IsNullOrEmpty(x)).ToArray();
//Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
//for (int i = 0; i < preRates.Length; i++)
//{
//    string key = preRates[i];
//    decimal value = Convert.ToDecimal(preRates[++i].Replace('.', ','));
//    rates.Add(key, value);
//}
//foreach (var item in rates)
//{
//    Console.WriteLine(item);
//}

            //string[] cuttedJson = json.Split('{', '}');
            //string jsonForDeserialize = "{" + cuttedJson[2] + "}";
            //Console.WriteLine(jsonForDeserialize);
            //var model = JsonConvert.DeserializeObject<ModelRates>(jsonForDeserialize);
            //Console.WriteLine(model.RUBUSD);