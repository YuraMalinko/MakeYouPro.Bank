using System.Net.Http.Headers;
using System.Text.Json;
using WebAPIClient;
using System.Net.Http.Headers;
using System.Text.Json;

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

await ProcessRepositoriesAsync(client);

static async Task ProcessRepositoriesAsync(HttpClient client)
{
    await using Stream stream =
 await client.GetStreamAsync( "https://currate.ru/api/?get=rates&pairs=RUBUSD,USDRUB,RUBEUR,EURRUB,RUBJPY,JPYRUB,RUBCNY,CNYRUB,RUBRSD,RSDRUB,RUBBGN,BGNRUB,RUBARS,ARSRUB,USDEUR,EURUSD,USDJPY,JPYUSD,USDCNY,CNYUSD,USDRSD,RSDUSD,USDBGN,BGNUSD,USDARS,ARSUSD,EURJPY,JPYEUR,EURCNY,CNYEUR,EURRSD,RSDEUR,EURBGN,BGNEUR,EURARS,ARSEUR,JPYCNY,CNYJPY,JPYRSD,RSDJPY,JPYBGN,BGNJPY,JPYARS,ARSJPY,CNYRSD,RSDCNY,CNYBGN,BGNCNY,CNYARS,ARSCNY,RSDBGN,BGNRSD,RSDARS,ARSRSD,BGNARS,ARSBGN&key=d93cb3151e643d1cbfe60829b8977980");

    var repositories =
        await JsonSerializer.DeserializeAsync<List<Repository>>(stream);
    foreach (var repo in repositories ?? Enumerable.Empty<Repository>())
        Console.Write(repo.name);
}
//RUBUSD,USDRUB,RUBEUR,EURRUB,RUBJPY,JPYRUB,RUBCNY,CNYRUB,RUBRSD,RSDRUB,RUBBGN,BGNRUB,RUBARS,ARSRUB,USDEUR,EURUSD,USDJPY,JPYUSD,USDCNY,CNYUSD,USDRSD,RSDUSD,USDBGN,BGNUSD,USDARS,ARSUSD,EURJPY,JPYEUR,EURCNY,CNYEUR,EURRSD,RSDEUR,EURBGN,BGNEUR,EURARS,ARSEUR,JPYCNY,CNYJPY,JPYRSD,
//RSDJPY,JPYBGN,BGNJPY,JPYARS,ARSJPY,CNYRSD,RSDCNY,CNYBGN,BGNCNY,CNYARS,ARSCNY,RSDBGN,BGNRSD,RSDARS,ARSRSD,BGNARS,ARSBGN