using Application.Exceptions;
using Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteError(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (DomainException ex)
            {
                await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                await WriteError(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static async Task WriteError(HttpContext context, HttpStatusCode status, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var payload = new { error = message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
