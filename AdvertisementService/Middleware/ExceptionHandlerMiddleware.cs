using System.Net;
using System.Text.Json;
using Domain.Models.Exceptions;

namespace API.Middleware
{
    internal class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ServiceException serviceException)
            {
                await HandleServiceException(context, serviceException);
            }
            catch (Exception exception)
            {
                await HandleUnknownException(context, exception);
            }
        }

        private static Task HandleServiceException(HttpContext context, ServiceException exception)
        {
            int statusCode = (int)exception.StatusCode;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            string result = JsonSerializer.Serialize(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            return context.Response.WriteAsync(result);
        }

        private static Task HandleUnknownException(HttpContext context, Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            string result = JsonSerializer.Serialize(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            return context.Response.WriteAsync(result);
        }
    }
}