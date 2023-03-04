using ASP.Services;
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
        private int z;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Dependency Injection
        // IServiceCollection - коллекция сервисов, много сервисов добавляется автоматически
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            //сервис MVC для обработки запросов
            //services.AddMvc();

            // добавление сервиса и его конкретную реализацию в коллекцию сервисов
            services.AddTransient<IMessageSender, EmailMessageSender>();
            services.AddTransient<TimeService>();
            services.AddTransient<MessageService>();

            //// добавление сущностей методом Transient (ICounter, CounterService конструктор - два объекта передаются)
            //services.AddTransient<ICounter, RandomCounter>();
            //services.AddTransient<CounterService>();

            //// добавление сущностей методом Scoped (ICounter, CounterService - один объект передается) 
            //services.AddScoped<ICounter, RandomCounter>();
            //services.AddScoped<CounterService>();

            // добавление сущностей методом Singleton (ICounter, CounterService - для всех запросов один объект RandomCounter) 
            services.AddSingleton<ICounter, RandomCounter>();
            services.AddSingleton<CounterService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // определяет обработку запроса
        // конвейер, все методы - кол-во компонентов конвеера
        // Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // int x = 5;
            // int y = 2;
            // int z = 0;
            // int x = 2;  //метод запускается только один раз, и определяет переменную
            // приложение в процессе разработки или развернуто 
            if (env.IsDevelopment())
            {
                // в процессе разработки, встраиваем в конвейер обработки запроса компонент вывода об ошибке
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // для пользователей сообщение об ошибке
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // переопределение имен файла по-умолчанию
            DefaultFilesOptions opt = new DefaultFilesOptions();
            opt.DefaultFileNames.Clear();   //удаление используемых названий по=умолчанию
            opt.DefaultFileNames.Add("hello.html");

            // файлы по-умолчанию
            app.UseDefaultFiles(opt);

            // статические файлы
            app.UseStaticFiles();

            //// типизация компонентом
            //app.UseMiddleware<TokenMiddleware>();
            // использование с переопредеением, также вызывает TokenMiddleware
            // любой образец для token
            //app.UseToken("654321"); 

            //// методы map могут быть вложенными
            //app.Map("/home", home =>
            //{
            //    // параметр home - IApplicationBuilder, у него вызываем метод map
            //    // путь запроса, по которому нужно обработать, деоегат - любое действие, которое принимает app builder и ничего не возвращает
            //    // в лямбда выражении определяется логика обработки запроса по этому пути
            //    home.Map("/index", (appBuilder) =>
            //    {
            //        // ответ пользователю по пути /home/index
            //        appBuilder.Run(async (context) =>
            //        {
            //            context.Response.ContentType = "text/html;charset=utf-8";
            //            await context.Response.WriteAsync("<h2>Home Page</h2>");
            //        });
            //    });
            //    // обработчик методом 
            //    // ответ пользователю по пути /home/about
            //    home.Map("/about", About);
            //});

            //// путь запроса, по которому нужно обработать, деоегат - любое действие, которое принимает app builder и ничего не возвращает
            //// в лямбда выражении определяется логика обработки запроса по этому пути
            //app.Map("/index", (appBuilder) =>
            //{
            //    // ответ пользователю по пути /index
            //    appBuilder.Run(async (context) =>
            //    {
            //        context.Response.ContentType = "text/html;charset=utf-8";
            //        await context.Response.WriteAsync("<h2>Home Page</h2>");
            //    });
            //});
            //// обработчик методом 
            //// ответ пользователю по пути /about
            //app.Map("/about", About);

            //// context - HttpContext, next - delegate task
            //app.Use(async (context, next) =>
            //{
            //    // когда приходит запрос, сначала компонент получает значение из компонента use, а затем передает в данном случае в run
            //    z = x * y;  // z = 10
            //    //await context.Response.WriteAsync("Hello");
            //    // вызов следующего компонента в конвеере через вызов делегата из параметра next
            //    await next();
            //    // управление обработкой запроса возвращается этому компоненту
            //    z = z * 5;  // z = 50
            //    await context.Response.WriteAsync($"result: {z}");
            //});

            // в качестве параметра принимает public delegate Task RequestDelegate(HttpContext context),
            // делегат использует один объект контекст, обращаемся к управлению ответом и в ответ на запрос приложения передается строка
            //app.Run(async (context) =>
            //{
            //    // x = x * 2;  //x = 4, configure определяет только один раз при запуске приложения, при каждом запросе x будет увеличиваться
            //    // await context.Response.WriteAsync($"x = {x}");
            //    // через контекст можно узнать все данные запроса и управлять ответом (напр хост, путь запроса, запрос-параметры строки запроса)
            //    string host = context.Request.Host.Value;           // хост - домен и порт
            //    string path = context.Request.Path;                 // путь запроса до знака ?
            //    string query = context.Request.QueryString.Value;   // параметры строки запроса после знака ?
            //    // можно отправить ответом html
            //    // кодировка....
            //    // установка типа содержимого ответа
            //    context.Response.ContentType = "text/html;charset=utf-8";
            //    await context.Response.WriteAsync($"<h3>Хост: {host}</h3>" + 
            //        $"<h3>Путь запроса: {path}</h3>" +
            //        $"<h3>Параметры строки запроса: {query}</h3>");
            //});

            // создание конвеера из компонентов Middleware, class RoutingMiddleware
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<AuthenticationMiddleware>();            
            app.UseMiddleware<RoutingMiddleware>();

            // app.Run(Handle);

            app.UseHttpsRedirection();

            

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        // middleware метод
        public async Task Handle(HttpContext context)
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
                $"<h3>Параметры строки запроса: {query}</h3>" +
                $"<h4>result: {z}</h4>");
            z = z * 2;  // z = 100
            // компонент завершил выполнение и управление обработкой запроса возвращается предыдущему компоненту
            await Task.FromResult(0);
        }

        private void About(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                MessageService sender = context.RequestServices.GetService<MessageService>();
                context.Response.ContentType = "text/html;charset=utf-8";
                await context.Response.WriteAsync($"<h2>About</h2>" + $"<h3>{sender.SendMessage()}</h3>");
            });
        }
    }
}
