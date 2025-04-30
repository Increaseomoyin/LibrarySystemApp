using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibrarySystemApp.Helper
{   
    //EXCEPTION HANDLER COMMENT
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError; //Default
            var title = "Internal Server Error";
            var details = exception.Message;

            switch (exception)
            {
                case AccessViolationException:
                    statusCode = (int)HttpStatusCode.Forbidden; //403
                    title = "Not Authorized";
                    break;
                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized; //401
                    title = "Not Authenticated";
                    break;
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound; //404
                    title = "Not Found";
                    break;
                case ArgumentException:
                    statusCode = (int)HttpStatusCode.BadRequest; //400
                    title = "Bad Request";
                    break;
            }

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "apllication/json";

            var problemDetails = new ProblemDetails()
            {
                Status = statusCode,
                Title = title,
                Detail = details,
                Instance = httpContext.Request.Path
            };
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
            return true;

        }
    }
}
