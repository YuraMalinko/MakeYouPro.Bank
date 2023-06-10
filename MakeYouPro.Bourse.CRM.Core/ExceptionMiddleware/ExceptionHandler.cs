using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class ExceptionHandler
    {
        private RequestDelegate _next;
        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AlreadyExistException ex)
            {
                var result = JsonSerializer.Serialize(new { Error = "AlreadyExistException Error "});
                context.Response.StatusCode = 409;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (NotFoundException ex)
            {
                var result = JsonSerializer.Serialize(new { Error = "NotFoundException Error: " + ex.EntityName });
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (ArgumentException ex)
            {
                var result = JsonSerializer.Serialize(new { Error = "ArgumentException Error " });
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (AccountArgumentException ex)
            {
                var result = JsonSerializer.Serialize(new { Error = "AccountDataException Error:" + ex.Message });
                context.Response.StatusCode = 412;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (FileNotFoundException ex)
            {
                var result = JsonSerializer.Serialize(new { Error = "FileNotFoundException Error:" + ex.Message });
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (AccountUnknownException ex)
            {
                var result = JsonSerializer.Serialize(new { Error = "AccountUnknownException Error:" + ex.Message });
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
        }
    }
}