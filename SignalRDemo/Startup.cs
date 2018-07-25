using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GaoJD.Club.Core;
using GaoJD.Club.Logger;
using GaoJD.Club.Redis;
using GaoJD.Club.Repository;
using GaoJD.Club.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRDemo.Common;

namespace SignalRDemo
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
            services.AddLogger();

            services.AddSession();

            services.AddMvc();

            //  services.AddSignalR();
            // services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(ILogicBase<>), typeof(LogicBase<>));
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            var builder = new ContainerBuilder();//实例化 AutoFac  容器   
            builder.Populate(services);
            builder.RegisterType<AppConfigurtaionServices>().SingleInstance();

            //注入Redis
            builder.RegisterType<RedisClient>().As<IRedisClient>().SingleInstance();

            builder.RegisterType(typeof(OpenConfiguration));

            Assembly[] asmLopgic = GetAllAssembly("*.Logic.dll").ToArray();
            builder.RegisterAssemblyTypes(asmLopgic)
               .Where(t => t.Name.EndsWith("Logic"))
               .AsImplementedInterfaces().AsSelf();


            //注册仓储
            //Assembly[] asmRepository = GetAllAssembly("*.Repository.dll").ToArray();
            //builder.RegisterAssemblyTypes(asmRepository)
            //   .Where(t => t.Name.EndsWith("Repository"))
            //   .AsImplementedInterfaces().AsSelf();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerDependency();

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
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
            app.UseStaticHttpContext();
            OpenConfiguration.Configure(Configuration);


            app.UseSession();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/chatHub");
            //});
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
