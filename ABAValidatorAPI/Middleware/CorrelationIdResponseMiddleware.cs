namespace ABAValidatorAPI.Middleware
{
    public class CorrelationIdResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers["X-Correlation-ID"] = context.TraceIdentifier;

            await _next(context);
        }
    }
}