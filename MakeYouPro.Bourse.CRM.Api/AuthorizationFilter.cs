using Microsoft.AspNetCore.Mvc.Filters;

namespace MakeYouPro.Bourse.CRM.Api
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
