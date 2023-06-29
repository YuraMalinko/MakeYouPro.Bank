using Microsoft.AspNetCore.Http;
using System.Text.Json;
using ILogger = NLog.ILogger;

namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class ExceptionHandler
    {
        private RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandler(RequestDelegate next, ILogger nLogger)
        {
            _next = next;
            _logger = nLogger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AlreadyExistException ex)
            {
                _logger.Error(ex, ex.StackTrace);

                var result = JsonSerializer.Serialize(new { Error = "AlreadyExistException Error " + ex.Message });
                context.Response.StatusCode = 409;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (NotFoundException ex)
            {
                _logger.Error(ex, ex.StackTrace);

                var result = JsonSerializer.Serialize(new { Error = "NotFoundException Error: " + ex.EntityName });
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, ex.StackTrace);

                var result = JsonSerializer.Serialize(new { Error = "ArgumentException Error " + ex.Message });
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (AccountArgumentException ex)
            {
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                };

                _logger.Error(ex, ex.StackTrace);

                string result = JsonSerializer.Serialize(new { Error = "AccountArgumentException Error:" + ex.Message }, options);
                context.Response.StatusCode = 412;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(ex, ex.StackTrace);

                var result = JsonSerializer.Serialize(new { Error = "FileNotFoundException Error:" + ex.Message });
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (AccountUnknownException ex)
            {
                _logger.Error(ex, ex.StackTrace);

                var result = JsonSerializer.Serialize(new { Error = "AccountUnknownException Error:" + ex.Message });
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (RegistrationException ex)
            {
                _logger.Error($"{ex.Message} {ex.StackTrace}");

                var result = JsonSerializer.Serialize(new { Error = "RegistrationException Error:" + ex.Message });
                context.Response.StatusCode = 412;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (AuthenticationException ex)
            {
                _logger.Error($"{ex.Message} {ex.StackTrace}");

                var result = JsonSerializer.Serialize(new { Error = "AuthenticationException Error:" + ex.Message });
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (AuthorizationException ex)
            {
                _logger.Error($"{ex.Message} {ex.StackTrace}");

                var result = JsonSerializer.Serialize(new { Error = "AuthorizationException Error:" + ex.Message });
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (RefreshTokenException ex)
            {
                _logger.Error($"{ex.Message} {ex.StackTrace}");

                var result = JsonSerializer.Serialize(new { Error = "RefreshTokenException Error:" + ex.Message });
                context.Response.StatusCode = 412;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
            catch (WritingDataToServerException ex)
            {
                _logger.Error($"{ex.Message} {ex.StackTrace}");

                var result = JsonSerializer.Serialize(new { Error = "WritingDataToServerException Error:" + ex.Message });
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }
        }

        private async Task MakingAnError(HttpContext context, Exception ex, int statusCode)
        {
            _logger.Error($"{ex.Message} {ex.StackTrace}");

            var result = JsonSerializer.Serialize(new { Error = "AuthenticationException Error:" + ex.Message });
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(result);
        }
    }
}