namespace API.Middleware
{
    public partial class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            HttpRequest request = httpContext.Request;
            HttpResponse response = httpContext.Response;

            LogRequestStarted(_logger, LogLevel.Information, request.Method, request.Path);

            try
            {
                httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:4200");
                await _next.Invoke(httpContext);

                if (response.StatusCode is >= 200 and <= 299)
                {
                    LogRequestFinished(_logger, LogLevel.Information, request.Method, request.Path, response.StatusCode);
                    return;
                }

                if (response.StatusCode >= 500)
                {
                    LogRequestFinished(_logger, LogLevel.Error, request.Method, request.Path, response.StatusCode);
                    return;
                }
                LogRequestFinished(_logger, LogLevel.Warning, request.Method, request.Path, response.StatusCode);
            }
            catch (Exception exception)
            {
                LogRequestException(_logger, LogLevel.Critical, request.Method, request.Path, exception);
                throw;
            }
        }

        [LoggerMessage(EventId = 1, Message = "Inbound HTTP {HttpMethod} {RequestPath} started")]
        private static partial void LogRequestStarted(ILogger logger, LogLevel logLevel, string httpMethod, PathString requestPath);

        [LoggerMessage(EventId = 2, Message = "Inbound HTTP {HttpMethod} {RequestPath} finished - {StatusCode}")]
        private static partial void LogRequestFinished(ILogger logger, LogLevel logLevel, string httpMethod, PathString requestPath, int StatusCode);

        [LoggerMessage(EventId = 3, Message = "Inbound HTTP {HttpMethod} {RequestPath} finished - Unhandled Exception")]
        private static partial void LogRequestException(ILogger logger, LogLevel logLevel, string httpMethod, PathString requestPath, Exception exception);
    }
}