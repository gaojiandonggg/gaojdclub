using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GaoJD.Club.Core
{
    /// <summary>
    /// 公共的拓展
    /// </summary>
    internal static class CommonExtensions
    {
        /// <summary>
        /// 自定义IEnumerable~T的ToString方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源</param>
        /// <param name="func">如果源是string类型func可以为null</param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> source, Func<T, string> func)
            where T : class
        {
            SqlServerUtilities.CheckNotEmpty(source, "source");
            string str1 = string.Empty;
            string temp = string.Empty;
            foreach (var t in source)
            {
                temp = (func == null ? (t as string) : func(t));
                if (!string.IsNullOrEmpty(temp))
                {
                    str1 += temp + ",";
                }
            }
            temp = null;
            return str1.TrimEnd(',');
        }
        /// <summary>
        /// 将两个集合组合在一起，两个集合的长度必须相等
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="list1">集合1</param>
        /// <param name="list2">集合2</param>
        /// <param name="func">组合的方法</param>
        /// <returns></returns>
        public static IList<TResult> Combine<T1, T2, TResult>(this IList<T1> list1, IList<T2> list2, Func<T1, T2, TResult> func)
            where T1 : class where T2 : class where TResult : class
        {
            SqlServerUtilities.CheckNotEmpty(list1, "list1");
            SqlServerUtilities.CheckNotEmpty(list2, "list2");
            SqlServerUtilities.CheckNotEmpty(func, "func");
            if (list1.Count != list2.Count)
            {
                throw new ArgumentException("list1与list2的数量不相等");
            }
            if (list1.Count == 0)
            {
                return null;
            }
            IList<TResult> result = new List<TResult>();
            for (int i = 0; i < list1.Count; i++)
            {
                result.Add(func(list1[i], list2[i]));
            }
            return result;
        }
        /// <summary>
        /// 执行sql语句或存储过程
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="parms">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public static int Execute(this IDbConnection connection, string sql, SqlParameter[] parms, CommandType commandType = default(CommandType))
        {
            DynamicParameters paramsValue = null;
            if (parms != null)
            {
                paramsValue = ConvertToDynamicParameters(parms);
            }
            if (commandType == CommandType.StoredProcedure)
            {
                /*由于现有业务中调用存储过程的方式是SqlQuery<T>("Pr_TC_Activities_HomeworkTool_GetCourseInfo @UserID", parms)
                 * 而Dapper调用存储过程不需要@UserID这个声明，所以此处做一个微处理*/
                if (!string.IsNullOrEmpty(sql))
                {
                    sql = sql.Trim();
                    int index = sql.IndexOf(' ');
                    if (index != -1)
                    {
                        sql = sql.Substring(0, index);
                        sql = sql.TrimEnd((char[])"\r\n".ToCharArray()); //目前项目中有换行符,所以过滤掉 wsa add
                    }
                }
            }
            int result = connection.Execute(sql, paramsValue, null, null, commandType);
            if (parms != null)
            {
                ConvertToSqlParameters(paramsValue, ref parms);
            }
            return result;
        }
        /// <summary>
        /// 通过sql语句或存储过程查询
        /// </summary>
        /// <typeparam name="T">类型对象</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">sql语句或存储过程</param>
        /// <param name="parms">参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbConnection connection, string sql, SqlParameter[] parms, CommandType commandType = default(CommandType))
        {
            DynamicParameters paramsValue = null;
            if (parms != null)
            {
                paramsValue = ConvertToDynamicParameters(parms);
            }
            if (commandType == CommandType.StoredProcedure)
            {
                /*由于现有业务中调用存储过程的方式是SqlQuery<T>("Pr_TC_Activities_HomeworkTool_GetCourseInfo @UserID", parms)
                 * 而Dapper调用存储过程不需要@UserID这个声明，所以此处做一个微处理*/
                if (!string.IsNullOrEmpty(sql))
                {
                    sql = sql.Trim();
                    int index = sql.IndexOf(' ');
                    if (index != -1)
                    {
                        sql = sql.Substring(0, index);
                        sql = sql.TrimEnd((char[])"\r\n".ToCharArray()); //目前项目中有换行符,所以过滤掉 wsa add
                    }
                }
            }
            IEnumerable<T> result = connection.Query<T>(sql, paramsValue, null, true, null, commandType);
            if (parms != null)
            {
                ConvertToSqlParameters(paramsValue, ref parms);
            }
            return result;
        }

        #region Private Method
        /// <summary>
        /// 通过SqlParameter数组转换成DynamicParameters
        /// </summary>
        /// <param name="sqlParms">sql参数</param>
        /// <returns></returns>
        public static DynamicParameters ConvertToDynamicParameters(SqlParameter[] sqlParms)
        {
            if (sqlParms == null)
            {
                return null;
            }
            DynamicParameters parms = new DynamicParameters();
            foreach (var p in sqlParms)
            {
                parms.Add(p.ParameterName, p.Value, p.DbType, p.Direction, p.Size);
            }
            return parms;
        }
        /// <summary>
        /// 将DynamicParameters中输出参数的值赋给SqlParameter[]中的输出参数
        /// </summary>
        /// <param name="parms">DynamicParameters参数</param>
        /// <param name="sqlParms">SqlParameter数组</param>
        /// <returns></returns>
        public static bool ConvertToSqlParameters(DynamicParameters parms, ref SqlParameter[] sqlParms)
        {
            if (parms == null || sqlParms == null)
            {
                return false;
            }
            foreach (var p in sqlParms)
            {
                if (p.Direction != ParameterDirection.Input)
                {//修改输出参数的值
                    p.Value = parms.Get<object>(p.ParameterName);
                }
            }
            return true;
        }
        #endregion
    }
}
