using System.Net;
using System.Text.Json;
using PersonalPlans.Core.CustomEntities;
using PersonalPlans.Core.Exceptions;
namespace PersonalPlans.Api.Filters;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) { _next=next; _logger=logger; }
    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (BusinessException ex)
        {
            context.Response.StatusCode=(int)ex.StatusCode;
            context.Response.ContentType="application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse { Status=(int)ex.StatusCode, Title=ex.StatusCode.ToString(), Message=ex.Message, Errors=ex.Details, TraceId=context.TraceIdentifier }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing request.");
            context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
            context.Response.ContentType="application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse { Status=500, Title="Internal Server Error", Message="An unexpected error occurred.", TraceId=context.TraceIdentifier }));
        }
    }
}
