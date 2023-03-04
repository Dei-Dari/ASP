using ASP.Services;
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
        private readonly IMessageSender _sender;

        public ErrorHandlerMiddleware(RequestDelegate next, IMessageSender sender)
        {
            _next = next;
            _sender = sender;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // можно передать здесь выполнение следующему компоненту, обращение идет до вызова делегата
            await _next(context);

            // выполнение уже при установленных статусных кодах
            context.Response.ContentType = "text/html;charset=utf-8";            
            if (context.Response.StatusCode == 403)
            {
                await context.Response.WriteAsync($"<h2>Access Denied</h2>" + $"<h3>{_sender.Send()}</h3>");
            }
            else if(context.Response.StatusCode == 404)
            {
                await context.Response.WriteAsync("Not Found");
            }
        }
    }
}
