using GaoJD.Club.Core.Logger;
using GaoJD.Club.Core.Utility;
using GaoJD.Club.Logic;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.LogstashLogger
{
    public class LogstashLogger<T> : BaseLogger<T>
    {
        public readonly LoggerConsumer loggerConsumer;

        public LogstashLogger(IConfiguration configuration) : base(configuration)
        {
            loggerConsumer = LoggerConsumer.Instance;
        }

        public override void Log(string Type, string message)
        {
            var log = this.GetLog(Type, message);
            loggerConsumer.Enqueue(log);
            base.ClearExtras();
        }

        public override void LogError(string message, Exception ex = null)
        {
            base.AddExtras("exception", Utils.GetException(ex));

            string msg = string.Concat("error:  ", this.LoggerName, "  ", message);
            Log log = this.GetLog("error", msg);
            loggerConsumer.Enqueue(log);
            base.ClearExtras();
        }

        public override void LogError(Exception ex)
        {
            if (ex != null)
            {
                LogError(" ", ex);
            }
        }


        public override void LogInfo(string message)
        {
            this.Log("info", message);
        }


        public override Log GetLog(string Type, string message)
        {
            var log = base.GetLog(Type, message);
            log.Opbusinessline = log.Opbusinessline.ToLower();
            log.Application = log.Application.ToLower();
            return log;
        }
    }
}
