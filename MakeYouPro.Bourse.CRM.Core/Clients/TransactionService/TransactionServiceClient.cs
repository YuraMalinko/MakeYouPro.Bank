using MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models;
using System.Net.Http.Json;
using System.Web;

namespace MakeYouPro.Bourse.CRM.Core.Clients.TransactionService
{
    public class TransactionServiceClient : ITransactionServiceClient
    {
        private HttpClient _client = new HttpClient();

        public TransactionServiceClient(string baseUri)
        {
            _client.BaseAddress = new Uri(baseUri);
        }

        public async Task<decimal> GetAccountBalanceAsync(int accountId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["accountId"] = accountId.ToString();
            string queryString = query.ToString();

            var response = await _client.GetAsync($"qwe?{queryString}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var body = await response.Content.ReadFromJsonAsync<decimal>();

                return body;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<int> CreateWithdrawTransactionAsync(WithdrawRequest transaction)
        {
            var response = await _client.PostAsJsonAsync("kek", transaction);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var body = await response.Content.ReadFromJsonAsync<int>();

                return body;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<int> CreateDepositTransactionAsync(DepositRequest transaction)
        {
            var response = await _client.PostAsJsonAsync("kek", transaction);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var body = await response.Content.ReadFromJsonAsync<int>();

                return body;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
