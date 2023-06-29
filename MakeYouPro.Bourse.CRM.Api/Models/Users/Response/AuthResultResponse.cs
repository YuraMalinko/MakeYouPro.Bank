using System.Text.Json.Serialization;

namespace MakeYouPro.Bourse.CRM.Api.Models.Users.Response
{
    public class AuthResultResponse
    {
        public string Token { get; set; }

        [JsonIgnore]
        public string TokenRefresh { get; set; }
    }
}
