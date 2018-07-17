using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Redis
{
    public static class RedisConnection
    {

        private static ConnectionMultiplexer _instance = null;

        private static readonly object _locker = new object();
       

        public static ConnectionMultiplexer GetInstance(string _redisConnectionString)
        {
            if (_instance == null || !_instance.IsConnected)
            {
                lock (_locker)
                {
                    int waitTime = 5000;
                    if (_instance == null)
                    {
                        ConfigurationOptions option = ConfigurationOptions.Parse(_redisConnectionString);
                        option.ConnectTimeout = waitTime;
                        option.ResponseTimeout = waitTime;
                        option.SyncTimeout = waitTime;
                        option.ConfigCheckSeconds = 18;
                        _instance = ConnectionMultiplexer.Connect(option);
                    }
                    else if (!_instance.IsConnected)
                    {
                        _instance.Close();
                        ConfigurationOptions option = ConfigurationOptions.Parse(_redisConnectionString);
                        option.ConnectTimeout = waitTime;
                        option.ResponseTimeout = waitTime;
                        option.SyncTimeout = waitTime;
                        option.ConfigCheckSeconds = 18;
                        _instance = ConnectionMultiplexer.Connect(option);
                        return _instance;
                    }
                }
            }
            return _instance;
        }

        static void _instance_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("连接失败");
            throw new NotImplementedException();
        }
    }
}
