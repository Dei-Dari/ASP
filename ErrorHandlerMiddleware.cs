using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // можно передать здесь выполнение следующему компоненту, обращение идет до вызова делегата
            await _next(context);

            // выполнение уже при установленных статусных кодах
            if (context.Response.StatusCode == 403)
            {
                await context.Response.WriteAsync("Access Denied");
            }
            else if(context.Response.StatusCode == 404)
            {
                await context.Response.WriteAsync("Not Found");
            }
        }
    }
}
