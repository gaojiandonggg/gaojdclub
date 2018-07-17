using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Utility
{
    public class AppSettings
    {
        public string CacheType { get; set; }
        public string CacheIP { get; set; }
        public string CachePort { get; set; }
        public string CachePassWord { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DataBase { get; set; }
        /// <summary>
        /// redis key 前缀
        /// </summary>
        public string RedisCustomKey { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string ApplicationID { get; set; }

        /// <summary>
        /// 加密Key
        /// </summary>
        public string OpenKey { get; set; }

    }
}
