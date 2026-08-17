using FurnitureERP.API.Common.Models;
using FurnitureERP.Application.Common.Exceptions;

namespace FurnitureERP.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                switch (ex)
                {
                    case ValidationExceptionApp:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;

                    case NotFoundExceptionApp:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;

                    case ConflictExceptionApp:
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        break;

                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                var response = new ApiErrorResponse
                {
                    Status = context.Response.StatusCode,
                    Message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);


            }
        }
    }
}
