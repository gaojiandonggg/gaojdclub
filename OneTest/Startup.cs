using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
using AspectCore.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GaoJD.Club.Core;
using GaoJD.Club.Core.Utility;
using GaoJD.Club.Logger;
using GaoJD.Club.LogstashLogger;
using GaoJD.Club.OneTest;
using GaoJD.Club.OneTest.Extensions;
using GaoJD.Club.OneTest.Filter;
using GaoJD.Club.OneTest.Middleware;
using GaoJD.Club.OneTest.Model;
using GaoJD.Club.Redis;
using GaoJD.Club.Repository;
using GaoJD.Club.Utility;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneTest.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace OneTest
{
    public class Startup
    {

        public static ILoggerRepository repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("Config/log4net.config"));
        }

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogstashLogger();
            services.AddLogger();
            services.AddCors();
            services.AddSession();
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            // services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
            //   .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");
            services.AddTransient<SampleInterface, SampleClass>();
            services.AddServiceEventBusExpression();  //事件总线
            services.AddSingleton<IEventHandler<MyTestEventData>, MyTestEventHandler>();
            services.AddSingleton<IEventHandler<MyTestEventData>, MyTestTwoEventHandler>();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(LogFilter));
                options.Filters.Add(typeof(ModelValidateFilter));
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                options.OutputFormatters.Add(new ProtobufFormatter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_0);



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new Info
                {
                    Version = "v2",
                    Title = "OneTest接口文档",
                    Description = "RESTful API for OneTest"

                });
                var basePath = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "GaoJD.Club.OneTest.xml");
                c.IncludeXmlComments(xmlPath);
                c.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数 
            });
            services.AddHttpContextAccessor();

            services.AddHttpClient();

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //   .AddCookie(o => o.LoginPath = new PathString("/login"));


            services.AddResponseCompression();

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });


            // services.AddTransient<SampleInterface, SampleClass>();
            // services.AddDynamicProxy();

            services.AddScoped(typeof(ILogicBase<>), typeof(LogicBase<>));
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            var builder = new ContainerBuilder();//实例化 AutoFac  容器   
            builder.Populate(services);


            builder.RegisterType<AppConfigurtaionServices>().SingleInstance();
            //注入Redis
            builder.RegisterType<RedisClient>().As<IRedisClient>().SingleInstance();
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
            //  builder.RegisterGeneric(typeof(LogicBase<>)).As(typeof(ILogicBase<>)).InstancePerDependency();
            //  builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerDependency();

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
                app.UseHsts();
                app.UseExceptionHandler("/Shared/Error");
            }

            app.UseCors(builder =>
            {
                //builder.WithOrigins("http://localhost:55811").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
            });
            app.UseStaticHttpContext();
            OpenConfiguration.Configure(Configuration);



            app.UseSession();

            app.UseHttpsRedirection();

            //异常中间件
            // app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "OneTest API V1");
                c.ShowRequestHeaders();
            });


            //app.UseAuthentication();


            app.UseIocConfiguration();


            app.UseResponseCompression();

            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHubNew>("/ChatHubNew");
            });
        }



    }
}
