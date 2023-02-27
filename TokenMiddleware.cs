using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP
{
    public class TokenMiddleware
    {
        // конструктор, параметр тип запрос делегат, через этот компонент можно обращаться к следующему методу в конвеере для обработки запроса
        // для сохранения объекта
        private readonly RequestDelegate _next;
        private readonly string _pattern;
        public TokenMiddleware(RequestDelegate next, string pattern)
        {
            _next = next;
            _pattern = pattern;
        }

        // обязательный метод Invoke, обработка запроса
        // когда компонет получит запрос на обработку, этот метод будет обрабатывать запрос
        public async Task InvokeAsync(HttpContext context)
        {
            // логика обработки запроса
            // параметр из строки запроса, query - все параметры строки запроса
            var token = context.Request.Query["token"];
            if(token!= _pattern)
            {
                // токен не корректен
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token is invalid");
            }
            else
            {
                // передача управления обработки запроса следующему компоненту
                await _next(context);
            }

            //// вызов следующего компонента в конвеере и передача обработки запроса
            //await _next(context);

        }

        

    }
    // методы расширения в классе для IApplicationBuilder
    public static class TokenExtensions
    {
        // метод usetoken,  используется IApplicationBuilder
        public static IApplicationBuilder UseToken(this IApplicationBuilder builder, string pattern)
        {
            // встраиваем в конвеер обработки запроса, при вызове метода, передается значение в конструктор
            return builder.UseMiddleware<TokenMiddleware>(pattern);
        }
    }
}
