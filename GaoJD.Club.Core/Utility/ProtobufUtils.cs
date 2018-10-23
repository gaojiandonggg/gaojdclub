using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ProtoBuf;
namespace GaoJD.Club.Core.Utility
{
    public static class ProtobufUtils
    {
        /// <summary>
		/// 将对象序列化到流中
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="destination"></param>
		/// <param name="instance"></param>
		public static void Serialize<T>(Stream destination, T instance)
        {

            Assert.NotNull(destination, nameof(destination));
            Serializer.Serialize(destination, instance);
            if (destination.CanSeek)
            {
                //将流重置到起点
                destination.Seek(0, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// 将流反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream source)
        {
            Assert.NotNull(source, nameof(source));
            return Serializer.Deserialize<T>(source);
        }

        /// <summary>
        /// 将流反序列化成匿名对象
        /// </summary>
        /// <typeparam name="T">匿名类型</typeparam>
        /// <param name="source"></param>
        /// <param name="anonymousType">匿名对象</param>
        /// <returns></returns>
        public static T DeserializeAnonymousType<T>(Stream source, T anonymousType)
            where T : class
        {
            Assert.NotNull(source, nameof(source));
            Assert.NotNull(anonymousType, nameof(anonymousType));
            Type type = anonymousType.GetType();
            return Serializer.Deserialize(type, source) as T;
        }
    }
}
