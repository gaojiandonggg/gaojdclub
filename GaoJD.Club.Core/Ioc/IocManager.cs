using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GaoJD.Club.Core.Ioc
{
    public class IocManager
    {
        private static AsyncLocal<IServiceProvider> _serviceProvider = new AsyncLocal<IServiceProvider>();

        /// <summary>
        /// 配置Ioc容器
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider.Value = serviceProvider;
        }

        /// <summary>
        /// 获取 ServiceProvider
        /// </summary>
        public static IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider.Value;
            }
        }
    }
}
