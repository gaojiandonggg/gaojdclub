using GaoJD.Club.Utility;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaoJD.Club.Redis
{
    public class RedisClient : IRedisClient
    {

        private static string _redisCustomKey = "";
        private AppConfigurtaionServices _AppConfigurtaionServices;
        private static string _redisConnectionString = "";
        public RedisClient(AppConfigurtaionServices AppConfigurtaionServices)
        {
            this._AppConfigurtaionServices = AppConfigurtaionServices;
            if (string.IsNullOrEmpty(_redisConnectionString))
            {
                _redisConnectionString = string.Concat(_AppConfigurtaionServices.GetAppSettings<AppSettings>("AppSettings").CacheIP, ",password=", _AppConfigurtaionServices.GetAppSettings<AppSettings>("AppSettings").CachePassWord, ",allowAdmin=true");
            }
            if (string.IsNullOrEmpty(_redisCustomKey))
            {
                _redisCustomKey = _AppConfigurtaionServices.GetAppSettings<AppSettings>("AppSettings").RedisCustomKey;
            }
        }


        #region 工具函数


        /// <summary>
        /// 合成key
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        private static string MergeKey(string key)
        {
            if (string.IsNullOrEmpty(_redisCustomKey))
                return key;
            return new StringBuilder().AppendFormat("{0}:{1}", _redisCustomKey, key).ToString();
        }

        /// <summary>
        /// 批量合成key
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        private static RedisKey[] ConvertRedisKeys(List<string> keys)
        {
            return keys.Select(key => (RedisKey)MergeKey(key)).ToArray();
        }

        /// <summary>
        /// 批量合成field
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private static RedisValue[] ConvertRedisFields(List<string> fields)
        {
            return fields.Select(field => (RedisValue)field).ToArray();
        }


        /// <summary>
        /// 获取指定数据库
        /// </summary>
        /// <param name="dbNum">数据库</param>
        /// <returns></returns>
        private IDatabase GetDatabase(int dbNum = -1)
        {
            return RedisConnection.GetInstance(_redisConnectionString).GetDatabase(dbNum);
        }
        #endregion

        #region 对象存储
        /// <summary>
        /// 以对象方式存储
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            return GetDatabase().StringSet(MergeKey(key), RedisUtility.ConvertToStr(value), expiry);
        }

        /// <summary>
        /// 以对象方式存储
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool Set<T>(int dbNum, string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            return GetDatabase().StringSet(MergeKey(key), RedisUtility.ConvertToStr(value), expiry);
        }

        /// <summary>
        /// 根据key获取对象方式存储的数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return RedisUtility.ConvertToEntity<T>(GetDatabase().StringGet(MergeKey(key)));
        }
        /// <summary>
        /// 根据key获取对象方式存储的数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(int dbNum, string key)
        {
            return RedisUtility.ConvertToEntity<T>(GetDatabase(dbNum).StringGet(MergeKey(key)));
        }

        /// <summary>
        /// 根据keys列表获取对象方式存储的数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="keys">键列表</param>
        /// <returns></returns>
        public List<T> GetList<T>(List<string> keys)
        {
            var values = GetDatabase().StringGet(ConvertRedisKeys(keys));
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i)).ToList();
        }

        /// <summary>
        /// 根据keys列表获取对象方式存储的数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public List<T> GetList<T>(int dbNum, List<string> keys)
        {
            var values = GetDatabase(dbNum).StringGet(ConvertRedisKeys(keys));
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i)).ToList();
        }

        /// <summary>
        /// 以对象方式存储(异步)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            return await GetDatabase().StringSetAsync(MergeKey(key), RedisUtility.ConvertToStr(value), expiry);
        }

        /// <summary>
        /// 以对象方式存储(异步)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(int dbNum, string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            return await GetDatabase(dbNum).StringSetAsync(MergeKey(key), RedisUtility.ConvertToStr(value), expiry);
        }

        /// <summary>
        /// 根据key获取对象方式存储的数据(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            return RedisUtility.ConvertToEntity<T>(await GetDatabase().StringGetAsync(MergeKey(key)));
        }

        /// <summary>
        /// 根据key获取对象方式存储的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(int dbNum, string key)
        {
            return RedisUtility.ConvertToEntity<T>(await GetDatabase(dbNum).StringGetAsync(MergeKey(key)));
        }
        #endregion

        #region String
        /// <summary>
        /// 存储字符串类型数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool SetString(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return GetDatabase().StringSet(MergeKey(key), value, expiry);
        }

        /// <summary>
        /// 存储字符串类型数据
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool SetString(int dbNum, string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return GetDatabase(dbNum).StringSet(MergeKey(key), value, expiry);
        }

        /// <summary>
        /// 根据key获取字符串类型数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return GetDatabase().StringGet(MergeKey(key));
        }

        /// <summary>
        /// 根据key获取字符串类型数据
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetString(int dbNum, string key)
        {
            return GetDatabase(dbNum).StringGet(MergeKey(key));
        }

        /// <summary>
        /// 为数字增长一(原子性)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public double Increment(string key)
        {
            return GetDatabase().StringIncrement(MergeKey(key));
        }

        /// <summary>
        /// 为数字增长一(原子性)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public double Increment(int dbNum, string key)
        {
            return GetDatabase(dbNum).StringIncrement(MergeKey(key));
        }

        /// <summary>
        /// 存储字符串类型数据(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return await GetDatabase().StringSetAsync(MergeKey(key), value, expiry);
        }

        /// <summary>
        /// 存储字符串类型数据(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> SetStringAsync(int dbNum, string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return await GetDatabase(dbNum).StringSetAsync(MergeKey(key), value, expiry);
        }

        /// <summary>
        /// 根据key获取字符串类型数据(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string key)
        {
            return await GetDatabase().StringGetAsync(MergeKey(key));
        }

        /// <summary>
        /// 根据key获取字符串类型数据(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(int dbNum, string key)
        {
            return await GetDatabase(dbNum).StringGetAsync(MergeKey(key));
        }

        /// <summary>
        /// 为数字增长一(原子性)(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<double> IncrementAsync(string key)
        {
            return await GetDatabase().StringIncrementAsync(MergeKey(key));
        }

        /// <summary>
        /// 为数字增长一(原子性)（异步）
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<double> IncrementAsync(int dbNum, string key)
        {
            return await GetDatabase(dbNum).StringIncrementAsync(MergeKey(key));
        }

        #endregion

        #region 操作key
        /// <summary>
        /// 判断指定key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool IsExist(string key)
        {
            return GetDatabase().KeyExists(MergeKey(key));
        }

        /// <summary>
        /// 判断指定key是否存在
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool IsExist(int dbNum, string key)
        {
            return GetDatabase(dbNum).KeyExists(MergeKey(key));
        }

        /// <summary>
        /// 判断指定key是否存在(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(string key)
        {
            return await GetDatabase().KeyExistsAsync(MergeKey(key));
        }

        /// <summary>
        /// 判断指定key是否存在(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(int dbNum, string key)
        {
            return await GetDatabase(dbNum).KeyExistsAsync(MergeKey(key));
        }

        /// <summary>
        /// 判断指定key下的field是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        public bool IsFieldExist(string key, string field)
        {
            return GetDatabase().HashExists(key, field);
        }

        /// <summary>
        /// 判断指定key下的field是否存在
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        public bool IsFieldExist(int dbNum, string key, string field)
        {
            return GetDatabase(dbNum).HashExists(key, field);
        }

        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return GetDatabase().KeyDelete(MergeKey(key));
        }

        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Remove(int dbNum, string key)
        {
            return GetDatabase(dbNum).KeyDelete(MergeKey(key));
        }

        /// <summary>
        /// 移除hash中的field
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        public bool RemoveField(string key, string field)
        {
            return GetDatabase().HashDelete(MergeKey(key), field);
        }

        /// <summary>
        /// 移除hash中的field
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        public bool RemoveField(int dbNum, string key, string field)
        {
            return GetDatabase().HashDelete(MergeKey(key), field);
        }

        /// <summary>
        /// 移除key(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Task<bool> RemoveAsync(string key)
        {
            return GetDatabase().KeyDeleteAsync(MergeKey(key));
        }

        /// <summary>
        /// 移除key(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Task<bool> RemoveAsync(int dbNum, string key)
        {
            return GetDatabase(dbNum).KeyDeleteAsync(MergeKey(key));
        }

        /// <summary>
        /// 批量移除key
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        public long RemoveList(List<string> keys)
        {
            return GetDatabase().KeyDelete(keys.Select(i => (RedisKey)MergeKey(i)).ToArray());
        }

        /// <summary>
        /// 批量移除key
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        public long RemoveList(int dbNum, List<string> keys)
        {
            return GetDatabase(dbNum).KeyDelete(keys.Select(i => (RedisKey)MergeKey(i)).ToArray());
        }

        /// <summary>
        /// 批量移除key(异步)
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        public async Task<long> RemoveListAsync(List<string> keys)
        {
            return await GetDatabase().KeyDeleteAsync(keys.Select(i => (RedisKey)MergeKey(i)).ToArray());
        }

        /// <summary>
        /// 批量移除key(异步)  
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="keys">键集合</param>
        /// <returns></returns> 
        public async Task<long> RemoveListAsync(int dbNum, List<string> keys)
        {
            return await GetDatabase(dbNum).KeyDeleteAsync(keys.Select(i => (RedisKey)MergeKey(i)).ToArray());
        }

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool SetExpire(string key, TimeSpan expiry)
        {
            return GetDatabase().KeyExpire(MergeKey(key), expiry);
        }

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool SetExpire(int dbNum, string key, TimeSpan expiry)
        {
            return GetDatabase(dbNum).KeyExpire(MergeKey(key), expiry);
        }

        /// <summary>
        /// 设置过期时间（异步）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> SetExpireAsync(string key, TimeSpan expiry)
        {
            return await GetDatabase().KeyExpireAsync(MergeKey(key), expiry);
        }

        /// <summary>
        /// 设置过期时间（异步）
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> SetExpireAsync(int dbNum, string key, TimeSpan expiry)
        {
            return await GetDatabase(dbNum).KeyExpireAsync(MergeKey(key), expiry);
        }

        #endregion

        #region Hash
        /// <summary>
        /// 存储指定类型数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetHash<T>(string key, string field, T value)
        {
            return GetDatabase().HashSet(MergeKey(key), field, RedisUtility.ConvertToStr(value));
        }


        public HashEntry[] HashGetAll(string key)
        {

            return GetDatabase().HashGetAll(key);
        }

        public void HashSet(string RedisHashKey, string friendlyName, string element)
        {
            GetDatabase().HashSet(RedisHashKey, friendlyName, element);
        }


        /// <summary>
        /// 存储指定类型数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool SetHash<T>(string key, string field, T value, TimeSpan expiry)
        {
            GetDatabase().HashSet(MergeKey(key), field, RedisUtility.ConvertToStr(value));
            return GetDatabase().KeyExpire(MergeKey(key), expiry);
        }

        /// <summary>
        /// 存储指定类型数据
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetHash<T>(int dbNum, string key, string field, T value)
        {
            return GetDatabase(dbNum).HashSet(MergeKey(key), field, RedisUtility.ConvertToStr(value));
        }

        /// <summary>
        /// 存储指定类型数据
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool SetHash<T>(int dbNum, string key, string field, T value, TimeSpan expiry)
        {
            GetDatabase(dbNum).HashSet(MergeKey(key), field, RedisUtility.ConvertToStr(value));
            return GetDatabase().KeyExpire(MergeKey(key), expiry);
        }

        /// <summary>
        /// 根据key和field获取指定类型数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        public T GetHash<T>(string key, string field)
        {
            return RedisUtility.ConvertToEntity<T>(GetDatabase().HashGet(MergeKey(key), field));
        }

        /// <summary>
        /// 根据key和field获取指定类型数据
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        public T GetHash<T>(int dbNum, string key, string field)
        {
            return RedisUtility.ConvertToEntity<T>(GetDatabase(dbNum).HashGet(MergeKey(key), field));
        }

        /// <summary>
        /// 根据key和fields获取指定类型数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="keys">键列表</param>
        /// <returns></returns>
        public List<T> GetHashList<T>(string key, List<string> fields)
        {
            var values = GetDatabase().HashGet(MergeKey(key), ConvertRedisFields(fields));
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i)).ToList();
        }

        /// <summary>
        /// 根据key和fields获取指定类型数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public List<T> GetHashList<T>(int dbNum, string key, List<string> fields)
        {
            var values = GetDatabase(dbNum).HashGet(MergeKey(key), ConvertRedisFields(fields));
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i)).ToList();
        }

        /// <summary>
        /// 根据key获取指定类型所有数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="keys">键列表</param>
        /// <returns></returns>
        public List<T> GetHashAll<T>(string key, List<string> fields)
        {
            var values = GetDatabase().HashGetAll(MergeKey(key));
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i.Value)).ToList();
        }

        /// <summary>
        /// 根据key获取指定类型所有数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public List<T> GetHashAll<T>(int dbNum, string key)
        {
            var values = GetDatabase(dbNum).HashGetAll(MergeKey(key));
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i.Value)).ToList();
        }

        /// <summary>
        /// 根据key获取指定类型所有数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="keys">键列表</param>
        /// <returns></returns>
        public async Task<List<T>> GetHashAllAsync<T>(string key, List<string> fields)
        {
            var values = await GetDatabase().HashGetAllAsync(MergeKey(key));
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i.Value)).ToList();
        }

        /// <summary>
        /// 根据key获取指定类型所有数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public async Task<List<T>> GetHashAllAsync<T>(int dbNum, string key)
        {
            var values = await GetDatabase(dbNum).HashGetAllAsync(MergeKey(key));
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i.Value)).ToList();
        }

        /// <summary>
        /// 根据key和fields获取指定类型数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="keys">键列表</param>
        /// <returns></returns>
        public async Task<List<T>> GetHashListAsync<T>(string key, List<string> fields)
        {
            var values = await GetDatabase().HashGetAsync(MergeKey(key), ConvertRedisFields(fields));
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i)).ToList();
        }

        /// <summary>
        /// 根据key和fields获取指定类型数据列表(value必须为同一类型)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public async Task<List<T>> GetHashListAsync<T>(int dbNum, string key, List<string> fields)
        {
            var values = await GetDatabase(dbNum).HashGetAsync(MergeKey(key), ConvertRedisFields(fields));
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i)).ToList();
        }

        /// <summary>
        /// 存储指定类型数据(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<bool> SetHashAsync<T>(string key, string field, T value)
        {
            return await GetDatabase().HashSetAsync(MergeKey(key), field, RedisUtility.ConvertToStr(value));
        }


        /// <summary>
        /// 获取所有hashtable中的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> HashGetAll<T>(string key)
        {
            var values = GetDatabase().HashGetAll(MergeKey(key)).Select(x => x.Value);
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i)).ToList();
        }

        /// <summary>
        /// 获取所有hashtable中的值(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> HashGetAllAsync<T>(string key)
        {
            var values = (await GetDatabase().HashGetAllAsync(MergeKey(key))).Select(x => x.Value);
            return values.Select(i => RedisUtility.ConvertToEntity<T>(i)).ToList();
        }

        /// <summary>
        /// 存储指定类型数据(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<bool> SetHashAsync<T>(int dbNum, string key, string field, T value)
        {
            return await GetDatabase(dbNum).HashSetAsync(MergeKey(key), field, RedisUtility.ConvertToStr(value));
        }

        /// <summary>
        /// 根据key和field获取指定类型数据(异步)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        public async Task<T> GetHashAsync<T>(string key, string field)
        {
            return RedisUtility.ConvertToEntity<T>(await GetDatabase().HashGetAsync(MergeKey(key), field));
        }

        /// <summary>
        /// 根据key和field获取指定类型数据(异步)
        /// </summary>
        /// <param name="dbNum">数据库号码</param>
        /// <param name="key">键</param>
        /// <param name="field">域</param>
        /// <returns></returns>
        public async Task<T> GetHashAsync<T>(int dbNum, string key, string field)
        {
            return RedisUtility.ConvertToEntity<T>(await GetDatabase(dbNum).HashGetAsync(MergeKey(key), field));
        }
        #endregion
    }
}
