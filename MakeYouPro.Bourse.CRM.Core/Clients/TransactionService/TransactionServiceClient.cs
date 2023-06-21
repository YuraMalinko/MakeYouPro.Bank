using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Core.Clients.TransactionService
{
    public class TransactionServiceClient : ITransactionServiceClient
    {
        private HttpClient _client = new HttpClient();

        public TransactionServiceClient(string baseUri)
        {
            _client.BaseAddress = new Uri(baseUri);
        }
    }
}
