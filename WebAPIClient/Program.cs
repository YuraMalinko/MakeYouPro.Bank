using Polly;
using Polly.Retry;
using WebAPIClient;

public class Program
{

    static int maxAttempts = 3;

    static void Main(string[] args)
    {
        HttpClient client;
        AsyncRetryPolicy retryPolicy = Policy.
            Handle<HttpRequestException>(exeption =>{return exeption.Message== "HttpRequestException"; }).
            WaitAndRetryAsync(maxAttempts, time => TimeSpan.FromSeconds(2*time));
        retryPolicy.ExecuteAsync(GetRates).Wait();
        Console.ReadLine();
    }
     async static Task GetRates()
        {
            int numberOfAttempt = 0;

            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("https://currate.ru/api/?get=rates&pairs=RUBUSD,USDRUB,RUBEUR,EURRUB,RUBJPY,JPYRUB,RUBCNY,CNYRUB,RUBRSD,RSDRUB,RUBBGN,BGNRUB,RUBARS,ARSRUB,USDEUR,EURUSD,USDJPY,JPYUSD,USDCNY,CNYUSD,USDRSD,RSDUSD,USDBGN,BGNUSD,USDARS,ARSUSD,EURJPY,JPYEUR,EURCNY,CNYEUR,EURRSD,RSDEUR,EURBGN,BGNEUR,EURARS,ARSEUR,JPYCNY,CNYJPY,JPYRSD,RSDJPY,JPYBGN,BGNJPY,JPYARS,ARSJPY,CNYRSD,RSDCNY,CNYBGN,BGNCNY,CNYARS,ARSCNY,RSDBGN,BGNRSD,RSDARS,ARSRSD,BGNARS,ARSBGN&key=d93cb3151e643d1cbfe60829b8977980");
                RateStorage.DeserializeJson(json);
                Dictionary<string, decimal> fullDictionaty = RateStorage.ModifyJsonToDictionaty(json);
            }
        }
}
