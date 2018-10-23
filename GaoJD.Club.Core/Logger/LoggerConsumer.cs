using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core.Logger
{
    public class LoggerConsumer : IDisposable
    {
        private readonly ConcurrentQueue<Log> _logQueue;

        public static readonly LoggerConsumer Instance;
        static LoggerConsumer()
        {
            Instance = new LoggerConsumer();
        }

        public void Enqueue(Log log)
        {
            _logQueue.Enqueue(log);
        }


        public LoggerConsumer()
        {
            _logQueue = new ConcurrentQueue<Log>();
        }

        public void Dispose()
        {

        }
    }
}
