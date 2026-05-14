namespace MediFlowApi.Middlewares
{
    public class ResponseTimeMiddleware
    {
        public readonly RequestDelegate next;
        public ResponseTimeMiddleware(RequestDelegate _next)
        {
            next= _next;
        }
        public async  Task  InvokeAsync(HttpContext context)
        {
            var watch=System.Diagnostics.Stopwatch.StartNew();
            await next(context);
            watch.Stop();
            var seconds = watch.ElapsedMilliseconds;
            Console.WriteLine($"[MediFlow Log]: {context.Request.Method} {context.Request.Path} took {seconds}ms");
        }
    }
}
