using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace task.Web.Api.Filters;

public sealed class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception, "Ошибка API: {Exception}. Путь: {Path}", context.Exception.Message, context.HttpContext.Request.Path);

        context.Result = new ObjectResult(new
        {
            error = "InternalServerError",
            message = "An unexpected server error occurred."
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;
    }
}
