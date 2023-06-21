using Microsoft.AspNetCore.Http;
using CoreRS.Logger;
using System.Net;

namespace CoreRS.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _log;

        public ExceptionMiddleware(RequestDelegate next, ILoggerManager log)
        {
            _log = log;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _log.LogError($"Something went wrong: {ex}");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(new ErrorDetailsModel()
            {
                StatusCode = context.Response.StatusCode,
                Message = "OOOOOOOps.....something went wrong... we are already working on it! ♥ ♥ ♥"
            }.ToString());
        }
    }
}
