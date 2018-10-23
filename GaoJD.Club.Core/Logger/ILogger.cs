using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core.Logger
{
    public interface ILogger<T>
    {
        void LogError(Exception ex);

        void LogError(string message, Exception ex);

        void LogInfo(string message);

        void Log(string Type, string message);

        string LoggerName { get; }

        ILogger<T> AddExtras(string key, object value);

    }
}
