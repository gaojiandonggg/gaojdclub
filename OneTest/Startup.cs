using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
using AspectCore.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GaoJD.Club.Core;
using GaoJD.Club.Logger;
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


        private DataBaseProvider _DataBaseProvider;
        public static ILoggerRepository repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _DataBaseProvider = new DataBaseProvider();

            // var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("Config/log4net.config"));
        }

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // if (_DataBaseProvider._isSqlServer)
            // {
            //     services.AddDbContext<ReadSqlServerContext>(opt =>
            //   opt.UseSqlServer(Configuration.GetConnectionString("ReadSqlServerConnection")));
            //     services.AddDbContext<WriteSqlServerContext>(opt =>
            // opt.UseSqlServer(Configuration.GetConnectionString("WriteSqlServerConnection")));
            // }
            // else if (_DataBaseProvider._isMySql)
            // {
            //     services.AddDbContext<TeacherClubContext>(options =>
            //options.UseMySql(Configuration.GetConnectionString("MySqlConnection")));
            // }

            services.AddLogger();
            services.AddCors();
            services.AddSession();
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            // services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
            //   .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");
            services.AddTransient<SampleInterface, SampleClass>();
            services.AddServiceEventBusExpression();
            services.AddSingleton<IEventHandler<MyTestEventData>, MyTestEventHandler>();
            services.AddSingleton<IEventHandler<MyTestEventData>, MyTestTwoEventHandler>();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(LogFilter));
                options.Filters.Add(typeof(ModelValidateFilter));
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));

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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(o => o.LoginPath = new PathString("/login"));


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



            //  builder.RegisterType<SampleClass>().As<SampleInterface>();
            //  builder.RegisterDynamicProxy();


            //builder.RegisterDynamicProxy(config =>
            //{
            //    // config
            //    //  config.Interceptors.AddTyped<CustomInterceptor>();
            //    config.Interceptors.Add(new InterceptorFactory(Predicates.ForMethod("*Query")));
            //});

            //注入Redis
            builder.RegisterType<RedisClient>().As<IRedisClient>().SingleInstance();
            builder.RegisterType(typeof(OpenConfiguration));
            Assembly[] asmLopgic = GetAllAssembly("*.Logic.dll").ToArray();
            builder.RegisterAssemblyTypes(asmLopgic)
               .Where(t => t.Name.EndsWith("Logic"))
               .AsImplementedInterfaces().AsSelf();

            //注册仓储
            Assembly[] asmRepository = GetAllAssembly("*.Repository.dll").ToArray();
            builder.RegisterAssemblyTypes(asmRepository)
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces().AsSelf();
            //  builder.RegisterGeneric(typeof(LogicBase<>)).As(typeof(ILogicBase<>)).InstancePerDependency();
            //  builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerDependency();
            builder.RegisterType<ApiAuthenticationFilter>().SingleInstance();

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

            //app.ApplicationServices.GetService

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


            //  app.UseAuthentication();


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        public static List<Assembly> GetAllAssembly(string dllName)
        {
            List<string> pluginpath = FindPlugin(dllName);
            var list = new List<Assembly>();
            foreach (string filename in pluginpath)
            {
                try
                {
                    string asmname = Path.GetFileNameWithoutExtension(filename);
                    if (asmname != string.Empty)
                    {
                        Assembly asm = Assembly.LoadFrom(filename);
                        list.Add(asm);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return list;
        }
        /// <summary>
        /// 查找所有插件的路径
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        private static List<string> FindPlugin(string dllName)
        {
            List<string> pluginpath = new List<string>();

            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] dllList = Directory.GetFiles(path, dllName);
            if (dllList.Length > 0)
            {
                pluginpath.AddRange(dllList.Select(item => Path.Combine(path, item.Substring(path.Length))));
            }
            return pluginpath;
        }

        /// <summary>  
        /// 获取程序集中的实现类对应的多个接口
        /// </summary>
        /// <param name="services"></param>  
        /// <param name="assemblyName">程序集</param>
        public void GetClassName(IServiceCollection services, string assemblyName)
        {
            Assembly[] asmLopgic = GetAllAssembly(assemblyName).ToArray();
            foreach (var assembly in asmLopgic)
            {
                var serviceTypes = assembly.GetTypes();
                foreach (var item in serviceTypes.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    foreach (var typeArray in interfaceType)
                    {
                        if (typeArray.Name.Contains(item.Name))
                        {
                            services.AddScoped(typeArray, item);
                        }
                    }
                }
            }
        }

    }
}
