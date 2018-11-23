using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GaoJD.Club.Core.Utility
{
    public static class AssemblyCommon
    {

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
        public static List<string> FindPlugin(string dllName)
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
        public static void GetClassName(IServiceCollection services, string assemblyName)
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
