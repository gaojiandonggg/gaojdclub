using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Utility
{
    /// <summary>
    /// 线程安全的缓存hashtable
    /// </summary>
    public sealed class CachedHashtable
    {
        private readonly Hashtable m_hashtable;
        private readonly object m_syncObj = new object();
        /// <summary>
        /// 构造CachedHashtable
        /// </summary>
        public CachedHashtable()
        {
            m_hashtable = new Hashtable();
        }
        /// <summary>
        /// 添加键值对，重复的键不会被添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(object key, object value)
        {
            lock (m_syncObj)
            {
                //重复的key会被忽略
                if (!this.ContainsKey(key))
                {
                    m_hashtable.Add(key, value);
                }
            }
        }
        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="key"></param>
        public void Remove(object key)
        {
            lock (m_syncObj)
            {
                m_hashtable.Remove(key);
            }
        }
        /// <summary>
        /// 移除所有元素
        /// </summary>
        public void Clear()
        {
            lock (m_syncObj)
            {
                m_hashtable.Clear();
            }
        }
        /// <summary>
        /// 是否包含特定的key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(object key)
        {
            return m_hashtable.ContainsKey(key);
        }
        /// <summary>
        /// 根据key获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(object key)
        {
            return m_hashtable[key];
        }

        /// <summary>
        /// 根据key获取/设置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[object key]
        {
            get
            {
                return m_hashtable[key];
            }
            set
            {
                lock (m_syncObj)
                {
                    if (this.ContainsKey(key))
                    {
                        m_hashtable[key] = value;
                    }
                }
            }
        }
    }
}
