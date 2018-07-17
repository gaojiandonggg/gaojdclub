using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GaoJD.Club.Redis
{
    public interface IRedisClient
    {
        #region 对象存储
        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool Set<T>(int dbNum, string key, T value, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 根据key获取存储的数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 根据key获取存储的数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        T Get<T>(int dbNum, string key);

        /// <summary>
        /// 根据keys获取存储的数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="keys">键列表</param>
        /// <returns></returns>
        List<T> GetList<T>(List<string> keys);

        /// <summary>
        /// 根据keys获取存储的数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="keys">键</param>
        /// <returns></returns>
        List<T> GetList<T>(int dbNum, List<string> keys);

        /// <summary>
        /// 以存储(异步)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 以存储(异步)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<bool> SetAsync<T>(int dbNum, string key, T value, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 根据key获取存储的数据(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 根据key获取存储的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(int dbNum, string key);
        #endregion

        #region String
        /// <summary>
        /// 存储字符串/数值类型数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool SetString(string key, string value, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 存储字符串/数值类型数据
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool SetString(int dbNum, string key, string value, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 根据key获取字符串类型数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string GetString(string key);

        /// <summary>
        /// 根据key获取字符串类型数据
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        string GetString(int dbNum, string key);

        /// <summary>
        /// 为数字增长一(原子性)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        double Increment(string key);

        /// <summary>
        /// 为数字增长一(原子性)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        double Increment(int dbNum, string key);

        /// <summary>
        /// 存储字符串类型数据(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 存储字符串类型数据(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<bool> SetStringAsync(int dbNum, string key, string value, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 根据key获取字符串类型数据(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<string> GetStringAsync(string key);

        /// <summary>
        /// 根据key获取字符串类型数据(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<string> GetStringAsync(int dbNum, string key);

        /// <summary>
        /// 为数字增长一(原子性)(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<double> IncrementAsync(string key);

        /// <summary>
        /// 为数字增长一(原子性)（异步）
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<double> IncrementAsync(int dbNum, string key);
        #endregion

        #region 操作key
        /// <summary>
        /// 判断指定key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool IsExist(string key);

        /// <summary>
        /// 判断指定key是否存在
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool IsExist(int dbNum, string key);

        /// <summary>
        /// 判断指定key是否存在(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<bool> IsExistAsync(string key);

        /// <summary>
        /// 判断指定key是否存在(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<bool> IsExistAsync(int dbNum, string key);

        /// <summary>
        /// 判断指定key下的field是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        bool IsFieldExist(string key, string field);

        /// <summary>
        /// 判断指定key下的field是否存在
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        bool IsFieldExist(int dbNum, string key, string field);

        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Remove(string key);

        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Remove(int dbNum, string key);

        /// <summary>
        /// 移除hash中的field
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        bool RemoveField(string key, string field);

        /// <summary>
        /// 移除hash中的field
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        bool RemoveField(int dbNum, string key, string field);

        /// <summary>
        /// 移除key(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// 移除key(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(int dbNum, string key);

        /// <summary>
        /// 批量移除key
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        long RemoveList(List<string> keys);

        /// <summary>
        /// 批量移除key
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        long RemoveList(int dbNum, List<string> keys);

        /// <summary>
        /// 批量移除key(异步)
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        Task<long> RemoveListAsync(List<string> keys);

        /// <summary>
        /// 批量移除key(异步)  
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="keys">键集合</param>
        /// <returns></returns> 
        Task<long> RemoveListAsync(int dbNum, List<string> keys);

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool SetExpire(string key, TimeSpan expiry);

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool SetExpire(int dbNum, string key, TimeSpan expiry);

        /// <summary>
        /// 设置过期时间（异步）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<bool> SetExpireAsync(string key, TimeSpan expiry);

        /// <summary>
        /// 设置过期时间（异步）
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<bool> SetExpireAsync(int dbNum, string key, TimeSpan expiry);

        #endregion

        #region Hash
        /// <summary>
        /// 存储指定类型数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool SetHash<T>(string key, string field, T value);

        /// <summary>
        /// 存储指定类型数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool SetHash<T>(string key, string field, T value, TimeSpan expiry);

        /// <summary>
        /// 存储指定类型数据
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool SetHash<T>(int dbNum, string key, string field, T value);

        /// <summary>
        /// 存储指定类型数据
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool SetHash<T>(int dbNum, string key, string field, T value, TimeSpan expiry);

        /// <summary>
        /// 根据key和field获取指定类型数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        T GetHash<T>(string key, string field);

        /// <summary>
        /// 根据key和field获取指定类型数据
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        T GetHash<T>(int dbNum, string key, string field);

        /// <summary>
        /// 根据key和field获取指定类型数据列表
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <param name="fields">字段列表</param>
        /// <returns></returns>
        List<T> GetHashList<T>(string key, List<string> fields);

        /// <summary>
        /// 根据key和field获取指定类型数据列表
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="fields">字段列表</param>
        /// <returns></returns>
        List<T> GetHashList<T>(int dbNum, string key, List<string> fields);

        /// <summary>
        /// 存储指定类型数据(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<bool> SetHashAsync<T>(string key, string field, T value);

        /// <summary>
        /// 存储指定类型数据(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<bool> SetHashAsync<T>(int dbNum, string key, string field, T value);

        /// <summary>
        /// 根据key和field获取指定类型数据(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        Task<T> GetHashAsync<T>(string key, string field);

        /// <summary>
        /// 根据key和field获取指定类型数据(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        Task<T> GetHashAsync<T>(int dbNum, string key, string field);

        ///// <summary>
        ///// 获取hashtable中所有key的value
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //HashEntry[] HashGetAll(string key);

        /// <summary>
        /// 获取hashtable中所有key的value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> HashGetAll<T>(string key);

        /// <summary>
        /// 存储hashtable
        /// </summary>
        /// <param name="RedisHashKey"></param>
        /// <param name="friendlyName"></param>
        /// <param name="element"></param>
        void HashSet(string RedisHashKey, string friendlyName, string element);

        /// <summary>
        /// 根据key和fields获取指定类型数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键列表</param>
        /// <returns></returns>
        Task<List<T>> GetHashListAsync<T>(string key, List<string> fields);

        /// <summary>
        /// 根据key和fields获取指定类型数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<List<T>> GetHashListAsync<T>(int dbNum, string key, List<string> fields);

        /// <summary>
        /// 获取所有hashtable中的值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<T>> HashGetAllAsync<T>(string key);

        /// <summary>
        /// 根据key获取指定类型所有数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键列表</param>
        /// <returns></returns>
        List<T> GetHashAll<T>(string key, List<string> fields);

        /// <summary>
        /// 根据key获取指定类型所有数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        List<T> GetHashAll<T>(int dbNum, string key);

        /// <summary>
        /// 根据key获取指定类型所有数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键列表</param>
        /// <returns></returns>
        Task<List<T>> GetHashAllAsync<T>(string key, List<string> fields);

        /// <summary>
        /// 根据key获取指定类型所有数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<List<T>> GetHashAllAsync<T>(int dbNum, string key);

        #endregion
    }
}
