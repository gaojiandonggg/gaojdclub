using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Extensions.Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GaoJD.Club.Core;
using GaoJD.Club.Core.Utility;
using GaoJD.Club.Logic;
using GaoJD.Club.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Swashbuckle.AspNetCore.Swagger;

namespace GateWay
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddOcelot(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("GateWay", new Info
                {
                    Version = "v2",
                    Title = "OneTest接口文档",
                    Description = "RESTful API for Two"
                });
                var basePath = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "GateWay.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddScoped(typeof(ILogicBase<>), typeof(LogicBase<>));
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

            var builder = new ContainerBuilder();//实例化 AutoFac  容器   
            builder.Populate(services);
            builder.RegisterType(typeof(OpenConfiguration));
            Assembly[] asmLopgic = AssemblyCommon.GetAllAssembly("*.Logic.dll").ToArray();
            builder.RegisterAssemblyTypes(asmLopgic)
               .Where(t => t.Name.EndsWith("Logic"))
               .AsImplementedInterfaces().AsSelf();

            //注册仓储
            Assembly[] asmRepository = AssemblyCommon.GetAllAssembly("*.Repository.dll").ToArray();
            builder.RegisterAssemblyTypes(asmRepository)
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces().AsSelf();

            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            OpenConfiguration.Configure(Configuration);
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            var logic = (IUserLogic)app.ApplicationServices.GetService(typeof(IUserLogic));

            var list = logic.GetAll();

            var apiList = new List<string>()
            {
                "One",
                "Two"
            };

            app.UseSwagger(c =>
            {

                c.RouteTemplate = "/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                apiList.ForEach(apiItem =>
                {
                    c.SwaggerEndpoint($"/{apiItem}/{apiItem}/swagger.json", apiItem);
                });
                c.ShowRequestHeaders();
            });

            app.UseOcelot().Wait();

        }
    }
}
