using log4net;

namespace GlobalAnalytics.API.ErrorHandling
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _logger = LogManager.GetLogger(typeof(GlobalExceptionHandlerMiddleware));

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Error("Unhandled exception occurred", ex);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\":\"An error occurred while processing your request.\"}");
            }
        }
    }
}
