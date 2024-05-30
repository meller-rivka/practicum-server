﻿using Newtonsoft.Json;

namespace Subscriber.WebWpi.Config
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> _looger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> looger)
        {
            this.next = next;
            _looger = looger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {

                await next(context);

                _looger.LogInformation($"the function: {context.Request.Method} finished ");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {

            var result = JsonConvert.SerializeObject(new { error = ex.Message });

            return context.Response.WriteAsync(result);
        }
    }
}
