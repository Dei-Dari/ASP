using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        //авторизация для упрощения по token
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];
            if (String.IsNullOrEmpty(token))
            {
                // статусный код - доступ к ресурсу запрещен
                context.Response.StatusCode = 403;
            }
            else
            {
                // передача управления следующему компоненту в конвеере
                await _next(context);
            }
        } 
    }
}
