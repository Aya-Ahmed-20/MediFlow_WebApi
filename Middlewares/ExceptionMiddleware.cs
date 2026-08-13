using System.Net;
using System.Text.Json;

namespace MediFlowApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // استخدام ?. لمنع الـ NullReferenceException إذا لم توجد InnerException
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, $"Unexpected Error Occurred: {errorMessage}");

                await HandelingExceptionAsync(context, ex);
            }
        }

        private static Task HandelingExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // إحضار أعمق InnerException للوصول لسبب SQL Server المباشر
            var actualError = ex.InnerException?.InnerException?.Message
                           ?? ex.InnerException?.Message
                           ?? ex.Message;

            var errorDetails = new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = "An internal error occurred and we are working to fix it.",
                DetailedError = actualError // سيطبع الخطأ الحقيقي من الداتا بيز مباشرة
            };

            var json = JsonSerializer.Serialize(errorDetails);
            return context.Response.WriteAsync(json);
        }
    }
}