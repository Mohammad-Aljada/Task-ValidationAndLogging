using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Task_ValidationAndLogging.Error
{
    public class GlobalExpetionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExpetionHandler> _logger;

        public GlobalExpetionHandler(ILogger<GlobalExpetionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "a occured error", exception.Message);
            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Detail = exception.Message,
            };
            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails);

            return true;

        }
    }
}
