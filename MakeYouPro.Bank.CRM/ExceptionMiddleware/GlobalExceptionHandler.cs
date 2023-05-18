using System.Text.Json;

namespace MakeYouPro.Bank.CRM.ExceptionMiddleware
{
    public class GlobalExceptionHandler
    {
        private RequestDelegate _next;
        public GlobalExceptionHandler(RequestDelegate next) 
        { 
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (MyCustomException ex)
            {
                var result = JsonSerializer.Serialize(new { Error = "MyCustomException Error: " + ex.Message });
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (NotImplementedException ex)
            {
                var result = JsonSerializer.Serialize(new { Error = "NotImplementedException Error: " + ex.Message });
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                var result = JsonSerializer.Serialize(new { Error = "Oops Error: " + ex.Message });
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
        }
    }
}
