using Serilog.Context;

namespace ABAValidatorAPI.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Use existing ID or generate a new one
            var correlationId =
                context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ??
                Guid.NewGuid().ToString();

            context.TraceIdentifier = correlationId;

            // Add Property to Log Context
            using (LogContext.PushProperty("TraceIdentifier", context.TraceIdentifier))
            {
                await _next(context);
            }
        }
    }
}
