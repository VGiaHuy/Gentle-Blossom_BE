namespace GentleBlossom_BE.Middleware
{
    using System.Net;
    using System.Text.Json;
    using GentleBlossom_BE.Exceptions;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // tiếp tục request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled exception: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                BadRequestException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                ForbiddenException => HttpStatusCode.Forbidden,
                ConflictException => HttpStatusCode.Conflict,
                InternalServerException => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError
            };

            var response = new
            {
                Success = false,
                Message = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

}
