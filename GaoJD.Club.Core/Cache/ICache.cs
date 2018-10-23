using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core.Cache
{
    public interface ICache
    {
        string GetString(string key);

        T GetValue<T>(string key);

        T GetHash<T>(string key, string field);

        void SetString(string key, string value, TimeSpan? timeout = null);

        void SetValue<T>(string key, T value, TimeSpan? timeout = null);

        void SetHash<T>(string key, string field, T value, TimeSpan? timeout = null);

        bool IsExists(string key);

        void Remove(string key);

        void RemoveField(string key, string field);
    }
}
