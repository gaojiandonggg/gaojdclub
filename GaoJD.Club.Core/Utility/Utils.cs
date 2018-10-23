using GaoJD.Club.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace GaoJD.Club.Core.Utility
{
    public static class Utils
    {
        /// <summary>
		/// 获取异常详细信息
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static ExceptionInfo GetException(Exception ex)
        {
            if (ex == null)
            {
                return new ExceptionInfo();
            }
            var error = new ExceptionInfo();
            error.Type = ex.GetType().FullName;
            error.Source = ex.Source ?? "no source";
            error.Message = ex.Message ?? "no message";
            error.StackTrace = ex.StackTrace ?? "no stacktrace";

            var inner = ex.InnerException;
            while (inner != null)
            {
                error.Type += $" || {inner.GetType().FullName}";
                error.Source += $" || {inner.Source ?? "no source"}";
                error.Message += $" || {inner.Message ?? "no message"}";
                error.StackTrace += $" || {inner.StackTrace ?? "no stacktrace"}";
                inner = inner.InnerException;
            }
            return error;
        }

        /// <summary>
        /// 检查参数的值是否为空
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="paramName">参数名称</param>
        public static void CheckNotEmpty<T>(T value, string paramName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
            if (value is string)
            {
                if (string.IsNullOrEmpty(value as string))
                {
                    throw new ArgumentNullException(paramName);
                }
            }
        }

        /// <summary>
        /// xml序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj)
            where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                string s1 = writer.ToString();
                return writer.ToString();
            }
        }
    }
}
