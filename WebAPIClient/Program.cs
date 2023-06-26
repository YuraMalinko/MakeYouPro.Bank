using WebAPIClient;

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


