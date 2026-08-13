using System.Net;

namespace MediFlowApi.Middlewares
{
    public class ExceptionHandler 
    {
        private readonly RequestDelegate _requestDelegate;
        public ExceptionHandler(RequestDelegate requestDelegate)
        { 
            _requestDelegate = requestDelegate;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _requestDelegate(context);
            }
            catch (Exception ex) 
            { 
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var res = new ErrorDetails
                {

                };
            }
        }
    }
}
