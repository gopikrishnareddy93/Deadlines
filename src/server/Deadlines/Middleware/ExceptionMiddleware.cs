using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Deadlines.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using NLog.Fluent;

namespace Deadlines.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Logger m_Logger = LogManager.GetLogger("Deadlines");

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BadRequestException)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest, "Bad request");
            }
            catch (NotFoundException)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.NotFound, "Not found");
            }
            catch (Exception ex)
            {
                m_Logger.Error($"ExceptionMiddleware.InvokeAsync: Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, HttpStatusCode.InternalServerError, "Internal Server Error from the custom middleware.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }

    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
