using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP
{
    public class RoutingMiddleware
    {
        // конструктор, получат объект делегат, объект передается локальной переменной
        private readonly RequestDelegate _next;
        public RoutingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // метод обработки запроса Invoke async
        public async Task InvokeAsync(HttpContext context)
        {
            // пользователю отправляется ответ в зависимости от запрошенного пути (путь запроса после домена и порта, параметры запроса после знака ?
            string path = context.Request.Path.Value.ToLower();
            // корень web приложения
            if (path == "/" || path == "/index")
            {
                // через контекст можно узнать все данные запроса и управлять ответом (напр хост, путь запроса, запрос-параметры строки запроса)
                string host = context.Request.Host.Value;           // хост - домен и порт
                // string path = context.Request.Path;                 // путь запроса до знака ?
                string query = context.Request.QueryString.Value;   // параметры строки запроса после знака ?
                                                                    // можно отправить ответом html
                                                                    // кодировка....
                                                                    // установка типа содержимого ответа
                context.Response.ContentType = "text/html;charset=utf-8";
                await context.Response.WriteAsync($"<h5>Хост: {host}</h5>" +
                    $"<h3>Путь запроса: {path}</h3>" +
                    $"<h3>Параметры строки запроса: {query}</h3>");
                    //$"<h4>result: {z}</h4>");
                //z = z * 2;  // z = 100
                //            // компонент завершил выполнение и управление обработкой запроса возвращается предыдущему компоненту
                //await Task.FromResult(0);
            }
            else if(path == "/about")
            {
                context.Response.ContentType = "text/html;charset=utf-8";
                await context.Response.WriteAsync("<h2>About</h2>");
            }
            else
            {
                // ресурс не найден
                context.Response.StatusCode = 404;
            }

        }
    }
}
