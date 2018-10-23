using Dapper;
using GaoJD.Club.Core.Cache;
using GaoJD.Club.Core.Utility;
using GaoJD.Club.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace GaoJD.Club.Core
{
    public static class SqlServerUtilities
    {
        /// <summary>
		/// 缓存反射的结果
		/// </summary>
		private static CachedHashtable m_cachedHashtable = new CachedHashtable();
        private static MemoryCache memoryCache = new MemoryCache();

        /// <summary>
        /// 检查参数的值是否为空
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="paramName">参数名称</param>
        public static void CheckNotEmpty<T>(T value, string paramName)
            where T : class
        {
            Utils.CheckNotEmpty<T>(value, paramName);
        }
        /// <summary>
        /// 获取实体映射的属性信息
        /// </summary>
        /// <typeparam name="T">实体信息</typeparam>
        /// <param name="obj">对象信息，如果为null，获取的属性不包含值</param>
        /// <returns></returns>
        public static MapperPropertyInfo[] GetMapperPropertyInfo<T>(T obj)
            where T : class
        {
            Type t = typeof(T);
            string key = t.FullName;

            if (obj == null && memoryCache.GetValue<MapperPropertyInfo[]>(key) != null)
            {
                return memoryCache.GetValue<MapperPropertyInfo[]>(key);
            }

            /*由于实例对象一致的几率很小，所以只针对类型反射的结果进行缓存*/
            if (obj == null && m_cachedHashtable[key] != null)
            {
                return (MapperPropertyInfo[])m_cachedHashtable[key];
            }
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);
            //初始化FastProperty列表
            Dictionary<PropertyInfo, FastProperty> fastProperties = properties.Select(p => new FastProperty(p)).ToDictionary(p => p.Property);
            List<MapperPropertyInfo> mappers = new List<MapperPropertyInfo>();
            for (int i = 0; i < properties.Length; i++)
            {
                MapperPropertyInfo mapper = new MapperPropertyInfo();
                //如果有KeyAttribute属性是主键
                if (properties[i].GetCustomAttribute(typeof(KeyAttribute)) != null)
                {
                    mapper.IsPrimaryKey = true;
                }
                //如果有DatabaseGeneratedAttribute属性并且DatabaseGeneratedAttribute的DatabaseGeneratedOption为Identity是自增
                var databaseAttr = properties[i].GetCustomAttribute(typeof(DatabaseGeneratedAttribute)) as DatabaseGeneratedAttribute;
                if (databaseAttr != null && databaseAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                {
                    mapper.IsIdentity = true;
                }
                var notMappedAttr = properties[i].GetCustomAttribute(typeof(NotMappedAttribute)) as NotMappedAttribute;
                if (notMappedAttr != null)
                {//忽略某些列
                    continue;
                }
                mapper.Name = properties[i].Name;
                if (obj != null)
                {

                    mapper.Value = fastProperties[properties[i]].Get(obj);
                    // mapper.Value = fastProperties[properties[i]].GetMethod(obj);
                    //fastProperties[properties[i]].Set(obj, (object)0);
                    mapper.Value = properties[i].GetValue(obj);
                    if (mapper.Value != null)
                    {
                        fastProperties[properties[i]].Set(obj, ((IConvertible)mapper.Value).ToType(properties[i].PropertyType, null));
                        // fastProperties[properties[i]].SetMethod(obj, ((IConvertible)mapper.Value).ToType(properties[i].PropertyType, null));
                    }

                }

                mappers.Add(mapper);
            }
            MapperPropertyInfo[] array = mappers.ToArray();
            if (obj == null)
            {
                m_cachedHashtable.Add(key, array);
                memoryCache.SetValue(key, array);
            }
            return array;
        }

        /// <summary>
        /// 获取参数化的结果
        /// </summary>
        /// <param name="mapper">映射对象的属性信息</param>
        /// <param name="paramsStartIndex">参数开始的索引</param>
        /// <param name="paramsValue">字段的值</param>
        /// <param name="fields">字段列表</param>
        /// <returns>参数化的结果</returns>
        public static string[] GetParameterized(MapperPropertyInfo[] mapper, int paramsStartIndex, ref DynamicParameters paramsValue, out string[] fields)
        {
            //默认不包含自增的列
            IEnumerable<MapperPropertyInfo> query = mapper.Where(p => !p.IsIdentity);
            int length = query.Count();
            string[] parameterized = new string[length];
            fields = new string[length];
            int i = 0;
            if (paramsValue == null)
            {
                paramsValue = new DynamicParameters();
            }
            foreach (var m in query)
            {
                //@p0,@p1...
                string str = "@p" + paramsStartIndex.ToString();
                parameterized[i] = str;
                fields[i] = m.Name;
                paramsValue.Add(str, m.Value);
                i++;
                paramsStartIndex++;
            }
            return parameterized;
        }
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="mapper">映射对象的属性信息</param>
        /// <returns></returns>
        public static string[] GetFields(MapperPropertyInfo[] mapper)
        {
            string[] fields = new string[mapper.Length];
            for (int i = 0; i < mapper.Length; i++)
            {
                fields[i] = mapper[i].Name;
            }
            return fields;
        }
        /// <summary>
        /// 获取参数化的where条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="paramsValue"></param>
        /// <param name="paramsLength"></param>
        /// <returns></returns>
        public static string GetWhereParameterized<T>(Expression<Func<T, bool>> where, ref DynamicParameters paramsValue, out int paramsLength)
        {
            if (where == null)
            {
                paramsLength = 0;
                return string.Empty;
            }
            List<object> parameters;
            string sql = where.ToParameterizedMSSqlString(out parameters);
            paramsLength = parameters.Count;
            if (paramsValue == null)
            {
                paramsValue = new DynamicParameters();
            }
            for (int i = 0; i < parameters.Count; i++)
            {
                paramsValue.Add("@p" + i.ToString(), parameters[i]);
            }
            return sql;
        }
        /// <summary>
        /// 获取类型对象中定义的主键
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">类型对象</param>
        /// <returns></returns>
        public static List<MapperPropertyInfo> GetPrimaryKeys<T>(T obj = null)
            where T : class
        {
            MapperPropertyInfo[] mapper = GetMapperPropertyInfo<T>(obj);
            var keys = mapper.Where(p => p.IsPrimaryKey == true).ToList();
            if (keys.Count == 0)
            {
                throw new Exception(string.Format("类型：{0}中没有主键", typeof(T).FullName));
            }
            return keys;
        }
        /// <summary>
        /// 设置主键的值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">类型对象</param>
        /// <param name="value">主键的值</param>
        public static void SetIdentityValue<T>(T obj, IConvertible value)
            where T : class
        {
            CheckNotEmpty<T>(obj, "obj");
            MapperPropertyInfo[] mapper = GetMapperPropertyInfo<T>(null);
            string identityKey = mapper.First(p => p.IsIdentity == true).Name;
            PropertyInfo pInfo = typeof(T).GetProperty(identityKey);
            FastProperty fastProperty = new FastProperty(pInfo);
            if (pInfo == null)
            {
                throw new Exception("未查询到主键: " + identityKey);
            }
            fastProperty.Set(obj, value.ToType(pInfo.PropertyType, null));
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="tableName">数据库表名称</param>
        /// <param name="dt">数据源</param>
        public static void AddList(string connectionString, string tableName, DataTable dt)
        {
            if (dt.Rows.Count == 0) return;
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dt);
            }
        }

    }
}
