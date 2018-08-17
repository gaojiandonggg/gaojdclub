using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GaoJD.Club.OneTest.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using WebApplication1.Interface;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;

        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {


            //services.ConfigureAll<MyOptions>(myOptions =>
            //{
            //    myOptions.Option1 = "ConfigureAll replacement value";
            //});
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.CookieHttpOnly = true;
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });


            var physicalProvider = hostingEnvironment.ContentRootFileProvider;
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetEntryAssembly());

            var compositeProvider = new CompositeFileProvider(physicalProvider, embeddedProvider);

            // choose one provider to use for the app and register it
            //services.AddSingleton<IFileProvider>(physicalProvider);
            //services.AddSingleton<IFileProvider>(embeddedProvider);
            services.AddSingleton<IFileProvider>(compositeProvider);


            services.Configure<MyOptions>(Configuration);

            services.Configure<MyOptions>("named_options_1", Configuration);


            services.AddSingleton<ConnectionStringResolverBase, DefaultConnectionStringResolver>();

            services.AddScoped<IPersonRepostitoy, PersonRepostitoy>();

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            services.Configure<MyOptions>("named_options_2", myOptions =>
            {
                myOptions.Option1 = "named_options_2_value1_from_action";
            });


            services.AddSingleton<ISession<User>, SecondUser>();


            services.AddMvc(option =>
            {
                option.Filters.Add(typeof(ModelValidateFilter));
            });


            var builder = new ContainerBuilder();//实例化 AutoFac  容器   
            builder.Populate(services);

            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            //var pathBase = Environment.GetEnvironmentVariable("ASPNETCORE_PATHBASE");
            //// 如果 ASPNETCORE_PATHBASE 的值不為空， 則使用 Pathbase 中間件
            //if (!string.IsNullOrEmpty(pathBase))
            //{
            //    app.UsePathBase(pathBase);
            //    // app.UsePathBase(new PathString(pathBase));
            //    Console.WriteLine("Hosting pathbase: " + pathBase);
            //}

            //app.Use((context, next) =>
            //{
            //    context.Request.PathBase = new PathString("/foo");
            //    return next();
            //});
            app.Use((context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/foo", out var remainder))
                {
                    context.Request.Path = "/default/index";
                }

                return next();
            });
            //env.EnvironmentName = EnvironmentName.Production;
            //loggerFactory
            //  .AddConsole(LogLevel.Warning)
            //  .AddDebug((category, logLevel) => (category.Contains("TodoApi") && logLevel >= LogLevel.Trace));
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }



            app.UseForwardedHeaders();


            //app.UseStatusCodePages(async context =>
            //{
            //    context.HttpContext.Response.ContentType = "text/plain";

            //    await context.HttpContext.Response.WriteAsync(
            //        "Status code page, status code: " +
            //        context.HttpContext.Response.StatusCode);
            //});

            // app.UseStatusCodePagesWithRedirects("/Home/Error");
            app.UseSession();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Requires the following import:
                    // using Microsoft.AspNetCore.Http;
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                },

                FileProvider = new PhysicalFileProvider(
       Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                RequestPath = "/MyStaticFiles"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                RequestPath = "/MyStaticFiles"
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
