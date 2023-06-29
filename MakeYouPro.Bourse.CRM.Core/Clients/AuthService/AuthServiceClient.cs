using MakeYouPro.Bourse.CRM.Core.Clients.AuthService.Models;
using System.Net.Http.Json;

namespace MakeYouPro.Bourse.CRM.Core.Clients.AuthService
{
    public class AuthServiceClient : IAuthServiceClient
    {
        private HttpClient _client = new HttpClient();

        public AuthServiceClient(string baseUri)
        {
            _client.BaseAddress = new Uri(baseUri);
        }

        public async Task RegisterAsync(UserRegisterRequest user)
        {
            await _client.PostAsJsonAsync("register", user);
        }

        public async Task<UserRegisterRequest> Login(UserRegisterRequest request)
        {
            var response = await _client.PostAsJsonAsync("login", request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var body = await response.Content.ReadFromJsonAsync<UserRegisterRequest>();

                return body;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
