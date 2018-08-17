using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public abstract class ConnectionStringResolverBase
    {

        protected ConnectionStringResolverBase() { }

        /// <summary>
        /// 解析读字符串
        /// </summary>
        /// <returns></returns>
        public abstract string ResolveReadConnectionString();

        /// <summary>
        /// 解析写字符串
        /// </summary>
        /// <returns></returns>
        public abstract string ResolveWriteConnectionString();
    }
}
