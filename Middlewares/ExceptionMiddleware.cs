using System.Net;
using System.Text.Json;

namespace MediFlowApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        // التعديل هنا: الـ RequestDelegate لازم يكون الأول
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
                // أضيفي $ قبل " لكي تعمل الـ {}
                _logger.LogError(ex, $"حدث خطأ غير متوقع: {ex.Message}");
                await HandelingExceptionAsync(context, ex);
            }
        }

        private static Task HandelingExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = "نعتذر يا هندسة، حدث خطأ داخلي ونعمل على إصلاحه.",
                DetailedError = ex.Message
            };

            var json = JsonSerializer.Serialize(errorDetails);
            return context.Response.WriteAsync(json);
        }
    }
}