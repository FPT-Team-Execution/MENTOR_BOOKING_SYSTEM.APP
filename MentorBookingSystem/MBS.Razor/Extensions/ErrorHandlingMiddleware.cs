using MBS.Services.Constants;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;

namespace MBS.Razor.Extensions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            //TODO: write log
            var errorResponse = new ErrorResponse { TimeStamp = DateTime.UtcNow, Error = ex.Message };
            var result = JsonConvert.SerializeObject(errorResponse);


            //Redirect
            context.Response.Redirect(RouteEndpoints.UnhandledException);
        }

        public class ErrorResponse
        {
            public int StatusCode { get; set; }

            public string Error { get; set; }

            public DateTime TimeStamp { get; set; }
        }
    }
}
