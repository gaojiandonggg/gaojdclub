using GaoJD.Club.Core.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GaoJD.Club.Core.Logger
{
    public class LoggerConsumer : IDisposable
    {
        private readonly ConcurrentQueue<Log> _logQueue;
        private readonly AutoResetEvent _event;
        private Task _consumerTask;
        private readonly List<Log> _buffer;
        private Action<List<Log>> _consumerAction;

        public static readonly LoggerConsumer Instance;
        static LoggerConsumer()
        {
            Instance = new LoggerConsumer();
        }

        public void Enqueue(Log log)
        {
            _logQueue.Enqueue(log);
            _event.Set();
        }

        public bool IsStart => _consumerAction != null;

        public LoggerConsumer()
        {
            _logQueue = new ConcurrentQueue<Log>();
            _event = new AutoResetEvent(false);
            _buffer = new List<Log>(1000);

        }

        public void Consumer(Action<List<Log>> action)
        {
            Assert.NotNull(action, nameof(action));
            if (Interlocked.CompareExchange(ref _consumerAction, action, null) == null)
            {
                _consumerAction = action;

                _consumerTask = Task.Factory.StartNew(() =>
                {
                    Log log;
                    while (true)
                    {
                        if (_logQueue.TryDequeue(out log))
                        {
                            if (log != null)
                            {
                                _buffer.Add(log);
                                if (_buffer.IsFull())
                                {
                                    try
                                    {
                                        _consumerAction(_buffer);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    _buffer.Clear();
                                }
                            }
                        }
                        else
                        {
                            if (!_buffer.IsEmpty())
                            {
                                try
                                {
                                    _consumerAction(_buffer);
                                }
                                catch (Exception ex)
                                {
                                }
                                _buffer.Clear();
                            }
                            _event.WaitOne();
                        }
                    }
                });

            }
        }

        public void Dispose()
        {
            if (_event != null)
            {
                _event.Dispose();
            }
        }
    }
}
