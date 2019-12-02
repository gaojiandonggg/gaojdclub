using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.Swagger;

namespace GateWayTwo
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Two", new Info
                {
                    Version = "v2",
                    Title = "OneTest接口文档",
                    Description = "RESTful API for Two"
                });
                var basePath = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "GateWayServiceTwo.xml");
                c.IncludeXmlComments(xmlPath);

            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((doc, request) =>
                {
                    HttpRequest _request = request;
                    StringValues referer;
                    _request.Headers.TryGetValue("Referer", out referer);
                    if (!string.IsNullOrEmpty(referer.ToString()) && referer.ToString().StartsWith("http://localhost:5002"))
                    {
                        doc.BasePath = "/Two";
                    }
                });

                c.RouteTemplate = "/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/Two/swagger.json", "Two");
                c.ShowRequestHeaders();
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            var features = app.ServerFeatures;
            var address = features.Get<IServerAddressesFeature>().Addresses.First();
            var uri = new Uri(address);

            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://10.98.24.53:8500/"));//请求注册的 Consul 地址
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{uri.Host}:{uri.Port}/home/health",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = "Two",
                Address = uri.Host,
                Port = uri.Port,
                Tags = new[] { $"urlprefix-/Two" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration);//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
        }
    }
}
