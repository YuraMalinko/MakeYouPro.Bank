using MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using System.Net;
using System.Net.Http.Json;

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
            //var query = HttpUtility.ParseQueryString(string.Empty);
            //query["accountId"] = accountId.ToString();
            //string queryString = query.ToString();

            var response = await _client.GetAsync($"balanse/{accountId}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var body = await response.Content.ReadFromJsonAsync<decimal>();

                return body;
            }
            else
            {
                throw new TransactionException();
            }
        }

        public async Task<int> CreateWithdrawTransactionAsync(WithdrawRequest transaction)
        {
            var response = await _client.PostAsJsonAsync(string.Empty, transaction);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var body = await response.Content.ReadFromJsonAsync<int>();

                return body;
            }
            else
            {
                throw new TransactionException();
            }
        }

        public async Task<int> CreateDepositTransactionAsync(DepositRequest transaction)
        {
            var response = await _client.PostAsJsonAsync(string.Empty, transaction);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var body = await response.Content.ReadFromJsonAsync<int>();

                return body;
            }
            else
            {
                throw new TransactionException();
            }
        }

        public async Task<List<int>> CreateTransferTransactionAsync(TransferRequest transaction)
        {
            var response = await _client.PostAsJsonAsync("transfer", transaction);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var body = await response.Content.ReadFromJsonAsync<List<int>>();

                return body;
            }
            else
            {
                throw new TransactionException();
            }
        }
    }
}
