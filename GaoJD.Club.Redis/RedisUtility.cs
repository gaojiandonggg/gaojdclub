using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaoJD.Club.Redis
{
    internal static class RedisUtility
    {
        /// <summary>
        /// 序列化数据为String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ConvertToStr<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 反序列化String为指定类型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ConvertToEntity<T>(string data)
        {
            if (string.IsNullOrEmpty(data))
                return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
