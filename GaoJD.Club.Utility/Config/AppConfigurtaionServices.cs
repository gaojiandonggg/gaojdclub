using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GaoJD.Club.Utility
{
    public class AppConfigurtaionServices
    {
        /// <summary>
        　　　　/// 获取自定义配置文件配置
        　　　　/// </summary>
        　　　　/// <typeparam name="T">配置模型</typeparam>
        　　　　/// <param name="key">根节点</param>
        　　　　/// <param name="configPath">配置文件名称</param>
        　　　　/// <returns></returns>
        public T GetAppSettings<T>(string key, string configPath = "appsettings.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = configPath, ReloadOnChange = true })
            .Build();

            var appconfig = new ServiceCollection()
            .AddOptions()
            .Configure<T>(config.GetSection(key))
            .BuildServiceProvider()
            .GetService<IOptions<T>>()
            .Value;

            return appconfig;
        }
        /// <summary>
        　　　　/// 获取自定义配置文件配置（异步方式）
        　　　　/// </summary>
        　　　　/// <typeparam name="T">配置模型</typeparam>
        　　　　/// <param name="key">根节点</param>
        　　　　/// <param name="configPath">配置文件名称</param>
        　　　　/// <returns></returns>
        public async Task<T> GetAppSettingsAsync<T>(string key, string configPath = "appsettings.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = configPath, ReloadOnChange = true })
            .Build();

            var appconfig = new ServiceCollection()
            .AddOptions()
            .Configure<T>(config.GetSection(key))
            .BuildServiceProvider()
            .GetService<IOptions<T>>()
            .Value;

            return await Task.Run(() => appconfig);
        }

        /// <summary>
        　　　　/// 获取自定义配置文件配置
        　　　　/// </summary>
        　　　　/// <typeparam name="T">配置模型</typeparam>
        　　　　/// <param name="key">根节点</param>
        　　　　/// <param name="configPath">配置文件名称</param>
        　　　　/// <returns></returns>
        public List<T> GetListAppSettings<T>(string key, string configPath = "appsettings.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = configPath, ReloadOnChange = true })
            .Build();

            var appconfig = new ServiceCollection()
            .AddOptions()
            .Configure<List<T>>(config.GetSection(key))
            .BuildServiceProvider()
            .GetService<IOptions<List<T>>>()
            .Value;

            return appconfig;
        }
        /// <summary>
        　　　　/// 获取自定义配置文件配置（异步方式）
        　　　　/// </summary>
        　　　　/// <typeparam name="T">配置模型</typeparam>
        　　　　/// <param name="key">根节点</param>
        　　　　/// <param name="configPath">配置文件名称</param>
        　　　　/// <returns></returns>
        public async Task<List<T>> GetListAppSettingsAsync<T>(string key, string configPath = "appsettings.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = configPath, ReloadOnChange = true })
            .Build();

            var appconfig = new ServiceCollection()
            .AddOptions()
            .Configure<List<T>>(config.GetSection(key))
            .BuildServiceProvider()
            .GetService<IOptions<List<T>>>()
            .Value;

            return await Task.Run(() => appconfig);
        }
    }
}
