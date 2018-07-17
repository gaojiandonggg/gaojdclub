using GaoJD.Club.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core
{
    public class DbContextFactory
    {
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        public static IDbProvider GetContext(WriteAndRead type)
        {
            if (type == WriteAndRead.Read)
                return new ReadSqlServerProvider(OpenConfiguration.Configuration["ConnectionStrings:ReadSqlServerConnection"].ToString());
            else
                return new WriteSqlServerProvider(OpenConfiguration.Configuration["ConnectionStrings:WriteSqlServerConnection"].ToString());

        }

        /// <summary>
        /// 获取上下文操作
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="OpertionType"></param>
        /// <returns></returns>
        public static IDbProvider CallContext<TEntity>(WriteAndRead type) where TEntity : class
        {
            var DbContext = GetContext(type);
            return (IDbProvider)DbContext;
        }
    }

    public enum WriteAndRead
    {
        Read = 1,
        Write = 2
    }
}
