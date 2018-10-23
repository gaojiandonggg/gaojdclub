using GaoJD.Club.Utility;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core.Cache
{
    public class MemoryCache : ICache
    {
        private readonly Microsoft.Extensions.Caching.Memory.MemoryCache _internal;

        public MemoryCache()
        {
            _internal = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
        }

        public T GetHash<T>(string key, string field)
        {
            Assert.NotNull(key, nameof(key));
            Assert.NotNull(field, nameof(field));

            var dicts = _internal.Get<Dictionary<string, object>>(key);
            if (dicts != null && dicts.ContainsKey(field))
            {
                return (T)dicts[field];
            }

            return default(T);
        }

        public string GetString(string key)
        {
            Assert.NotNull(key, nameof(key));
            return _internal.Get<string>(key);
        }

        public T GetValue<T>(string key)
        {
            Assert.NotNull(key, nameof(key));
            return _internal.Get<T>(key);
        }

        public bool IsExists(string key)
        {
            Assert.NotNull(key, nameof(key));
            return _internal.Get(key) != null;
        }

        public void Remove(string key)
        {
            Assert.NotNull(key, nameof(key));
            _internal.Remove(key);
        }

        public void RemoveField(string key, string field)
        {
            Assert.NotNull(key, nameof(key));
            Assert.NotNull(field, nameof(field));

            var dicts = _internal.Get<Dictionary<string, object>>(key);
            if (dicts != null && dicts.ContainsKey(field))
            {
                dicts.Remove(field);
                _internal.Set(key, dicts);
            }
        }

        public void SetHash<T>(string key, string field, T value, TimeSpan? timeout = null)
        {
            Assert.NotNull(key, nameof(key));
            Assert.NotNull(field, nameof(field));

            var dicts = _internal.Get<Dictionary<string, object>>(key) ?? new Dictionary<string, object>();
            if (dicts.ContainsKey(field))
            {
                throw new ArgumentException("field is exists");
            }
            dicts.Add(field, value);

            MemoryCacheEntryOptions options = null;
            if (timeout.HasValue)
            {
                options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(timeout.Value);
            }
            _internal.Set(key, dicts, options);
        }

        public void SetString(string key, string value, TimeSpan? timeout = null)
        {
            MemoryCacheEntryOptions options = null;
            if (timeout.HasValue)
            {
                options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(timeout.Value);
            }
            _internal.Set(key, value, options);
        }

        public void SetValue<T>(string key, T value, TimeSpan? timeout = null)
        {
            MemoryCacheEntryOptions options = null;
            if (timeout.HasValue)
            {
                options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(timeout.Value);
            }
            _internal.Set(key, value, options);
        }
    }
}
