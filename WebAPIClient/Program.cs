using Polly;
using Polly.Retry;
using WebAPIClient;

public class Program
{
    static int maxAttempts = 2;
    static void Main(string[] args)
    {
        //HttpClient client;
        int numberOfAttempt = 0;
        PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(3));
        AsyncRetryPolicy retryPolicy = Policy.
             Handle<HttpRequestException>(exeption => { return exeption.Message == "HttpRequestException"; })
            .Or<Exception>(exeption => { return exeption.Message == "unexpected error"; })
            .WaitAndRetryAsync(maxAttempts, time => TimeSpan.FromSeconds(2*time));

        retryPolicy.ExecuteAsync(GetAndSaveRates).Wait();
        //async void CreatingRegularRatesGetting()
        //{
        //    while (await timer.WaitForNextTickAsync() && numberOfAttempt <= maxAttempts)
        //    {
        //        Console.WriteLine(numberOfAttempt);
        //        retryPolicy.ExecuteAsync(GetRates);
        //        numberOfAttempt++;
        //    }
        //}
        //CreatingRegularRatesGetting();
        //Console.ReadLine();
    }
    async static Task GetAndSaveRates()
    {
        using (var client = new HttpClient())
        {
            var json = await client.GetStringAsync("https://currate.ru/api/?get=rates&pairs=RUBUSD,USDRUB,RUBEUR,EURRUB,RUBJPY,JPYRUB,RUBCNY,CNYRUB,RUBRSD,RSDRUB,RUBBGN,BGNRUB,RUBARS,ARSRUB,USDEUR,EURUSD,USDJPY,JPYUSD,USDCNY,CNYUSD,USDRSD,RSDUSD,USDBGN,BGNUSD,USDARS,ARSUSD,EURJPY,JPYEUR,EURCNY,CNYEUR,EURRSD,RSDEUR,EURBGN,BGNEUR,EURARS,ARSEUR,JPYCNY,CNYJPY,JPYRSD,RSDJPY,JPYBGN,BGNJPY,JPYARS,ARSJPY,CNYRSD,RSDCNY,CNYBGN,BGNCNY,CNYARS,ARSCNY,RSDBGN,BGNRSD,RSDARS,ARSRSD,BGNARS,ARSBGN&key=d93cb3151e643d1cbfe60829b8977980");
            
            var model = RateStorage.DeserializeJson(json);
            RateStorage.ConvertClassToDictionary(model);
            model.data.DateTime = DateTime.Now; 
        }
    }
}
