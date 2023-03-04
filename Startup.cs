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
        // IServiceCollection - ��������� ��������, ����� �������� ����������� �������������
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            //������ MVC ��� ��������� ��������
            //services.AddMvc();

            // ���������� ������� � ��� ���������� ���������� � ��������� ��������
            services.AddTransient<IMessageSender, EmailMessageSender>();
            services.AddTransient<TimeService>();
            services.AddTransient<MessageService>();

            //// ���������� ��������� ������� Transient (ICounter, CounterService ����������� - ��� ������� ����������)
            //services.AddTransient<ICounter, RandomCounter>();
            //services.AddTransient<CounterService>();

            //// ���������� ��������� ������� Scoped (ICounter, CounterService - ���� ������ ����������) 
            //services.AddScoped<ICounter, RandomCounter>();
            //services.AddScoped<CounterService>();

            // ���������� ��������� ������� Singleton (ICounter, CounterService - ��� ���� �������� ���� ������ RandomCounter) 
            services.AddSingleton<ICounter, RandomCounter>();
            services.AddSingleton<CounterService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ���������� ��������� �������
        // ��������, ��� ������ - ���-�� ����������� ��������
        // Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // int x = 5;
            // int y = 2;
            // int z = 0;
            // int x = 2;  //����� ����������� ������ ���� ���, � ���������� ����������
            // ���������� � �������� ���������� ��� ���������� 
            if (env.IsDevelopment())
            {
                // � �������� ����������, ���������� � �������� ��������� ������� ��������� ������ �� ������
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // ��� ������������� ��������� �� ������
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // ��������������� ���� ����� ��-���������
            DefaultFilesOptions opt = new DefaultFilesOptions();
            opt.DefaultFileNames.Clear();   //�������� ������������ �������� ��=���������
            opt.DefaultFileNames.Add("hello.html");

            // ����� ��-���������
            app.UseDefaultFiles(opt);

            // ����������� �����
            app.UseStaticFiles();

            //// ��������� �����������
            //app.UseMiddleware<TokenMiddleware>();
            // ������������� � ���������������, ����� �������� TokenMiddleware
            // ����� ������� ��� token
            //app.UseToken("654321"); 

            //// ������ map ����� ���� ����������
            //app.Map("/home", home =>
            //{
            //    // �������� home - IApplicationBuilder, � ���� �������� ����� map
            //    // ���� �������, �� �������� ����� ����������, ������� - ����� ��������, ������� ��������� app builder � ������ �� ����������
            //    // � ������ ��������� ������������ ������ ��������� ������� �� ����� ����
            //    home.Map("/index", (appBuilder) =>
            //    {
            //        // ����� ������������ �� ���� /home/index
            //        appBuilder.Run(async (context) =>
            //        {
            //            context.Response.ContentType = "text/html;charset=utf-8";
            //            await context.Response.WriteAsync("<h2>Home Page</h2>");
            //        });
            //    });
            //    // ���������� ������� 
            //    // ����� ������������ �� ���� /home/about
            //    home.Map("/about", About);
            //});

            //// ���� �������, �� �������� ����� ����������, ������� - ����� ��������, ������� ��������� app builder � ������ �� ����������
            //// � ������ ��������� ������������ ������ ��������� ������� �� ����� ����
            //app.Map("/index", (appBuilder) =>
            //{
            //    // ����� ������������ �� ���� /index
            //    appBuilder.Run(async (context) =>
            //    {
            //        context.Response.ContentType = "text/html;charset=utf-8";
            //        await context.Response.WriteAsync("<h2>Home Page</h2>");
            //    });
            //});
            //// ���������� ������� 
            //// ����� ������������ �� ���� /about
            //app.Map("/about", About);

            //// context - HttpContext, next - delegate task
            //app.Use(async (context, next) =>
            //{
            //    // ����� �������� ������, ������� ��������� �������� �������� �� ���������� use, � ����� �������� � ������ ������ � run
            //    z = x * y;  // z = 10
            //    //await context.Response.WriteAsync("Hello");
            //    // ����� ���������� ���������� � �������� ����� ����� �������� �� ��������� next
            //    await next();
            //    // ���������� ���������� ������� ������������ ����� ����������
            //    z = z * 5;  // z = 50
            //    await context.Response.WriteAsync($"result: {z}");
            //});

            // � �������� ��������� ��������� public delegate Task RequestDelegate(HttpContext context),
            // ������� ���������� ���� ������ ��������, ���������� � ���������� ������� � � ����� �� ������ ���������� ���������� ������
            //app.Run(async (context) =>
            //{
            //    // x = x * 2;  //x = 4, configure ���������� ������ ���� ��� ��� ������� ����������, ��� ������ ������� x ����� �������������
            //    // await context.Response.WriteAsync($"x = {x}");
            //    // ����� �������� ����� ������ ��� ������ ������� � ��������� ������� (���� ����, ���� �������, ������-��������� ������ �������)
            //    string host = context.Request.Host.Value;           // ���� - ����� � ����
            //    string path = context.Request.Path;                 // ���� ������� �� ����� ?
            //    string query = context.Request.QueryString.Value;   // ��������� ������ ������� ����� ����� ?
            //    // ����� ��������� ������� html
            //    // ���������....
            //    // ��������� ���� ����������� ������
            //    context.Response.ContentType = "text/html;charset=utf-8";
            //    await context.Response.WriteAsync($"<h3>����: {host}</h3>" + 
            //        $"<h3>���� �������: {path}</h3>" +
            //        $"<h3>��������� ������ �������: {query}</h3>");
            //});

            // �������� �������� �� ����������� Middleware, class RoutingMiddleware
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

        // middleware �����
        public async Task Handle(HttpContext context)
        {
            // ����� �������� ����� ������ ��� ������ ������� � ��������� ������� (���� ����, ���� �������, ������-��������� ������ �������)
            string host = context.Request.Host.Value;           // ���� - ����� � ����
            string path = context.Request.Path;                 // ���� ������� �� ����� ?
            string query = context.Request.QueryString.Value;   // ��������� ������ ������� ����� ����� ?
                                                                // ����� ��������� ������� html
                                                                // ���������....
                                                                // ��������� ���� ����������� ������
            context.Response.ContentType = "text/html;charset=utf-8";
            await context.Response.WriteAsync($"<h3>����: {host}</h3>" +
                $"<h3>���� �������: {path}</h3>" +
                $"<h3>��������� ������ �������: {query}</h3>" +
                $"<h4>result: {z}</h4>");
            z = z * 2;  // z = 100
            // ��������� �������� ���������� � ���������� ���������� ������� ������������ ����������� ����������
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
