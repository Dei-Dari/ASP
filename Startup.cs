using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            //сервис MVC для обработки запросов
            // services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // определяет обработку запроса
        // конвеер, все методы - кол-во компонентов конвеера
        // Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // int x = 2;  //метод запускается только один раз, и определяет переменную
            // приложение в процессе разработки или развернуто 
            if (env.IsDevelopment())
            {
                // в процессе разработки, встраиваем в конвеер обработки запроса компонент вывода об ошибке
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // для пользователей сообщение об ошибке
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // в качестве параметра принимает public delegate Task RequestDelegate(HttpContext context),
            // делегат использует один объект контекст, обращаемся к управлению ответом и в ответ на запрос приложения передается строка
            app.Run(async (context) =>
            {
                // x = x * 2;  //x = 4, configure определяет только один раз при запуске приложения, при каждом запросе x будет увеличиваться
                // await context.Response.WriteAsync($"x = {x}");
                // через контекст можно узнать все данные запроса и управлять ответом (напр хост, путь запроса, запрос-параметры строки запроса)
                string host = context.Request.Host.Value;           // хост - домен и порт
                string path = context.Request.Path;                 // путь запроса до знака ?
                string query = context.Request.QueryString.Value;   // параметры строки запроса после знака ?
                // можно отправить ответом html
                // кодировка....
                // установка типа содержимого ответа
                context.Response.ContentType = "text/html;charset=utf-8";
                await context.Response.WriteAsync($"<h3>Хост: {host}</h3>" + 
                    $"<h3>Путь запроса: {path}</h3>" +
                    $"<h3>Параметры строки запроса: {query}</h3>");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        // middleware
        private async Task Handle(HttpContext context)
        {
            // через контекст можно узнать все данные запроса и управлять ответом (напр хост, путь запроса, запрос-параметры строки запроса)
            string host = context.Request.Host.Value;           // хост - домен и порт
            string path = context.Request.Path;                 // путь запроса до знака ?
            string query = context.Request.QueryString.Value;   // параметры строки запроса после знака ?
                                                                // можно отправить ответом html
                                                                // кодировка....
                                                                // установка типа содержимого ответа
            context.Response.ContentType = "text/html;charset=utf-8";
            await context.Response.WriteAsync($"<h3>Хост: {host}</h3>" +
                $"<h3>Путь запроса: {path}</h3>" +
                $"<h3>Параметры строки запроса: {query}</h3>");
        }
    }
}
