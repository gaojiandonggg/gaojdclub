using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Utility
{

    /// <summary>
    /// 全局IConfiguration
    /// </summary>
    public class OpenConfiguration
    {
        private static IConfiguration _configuration;


        public static void Configure(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    throw new NullReferenceException("未初始化配置IConfiguration");
                }
                return _configuration;
            }
        }
    }
}
