using GaoJD.Club.Core.Logger;
using GaoJD.Club.Core.Utility;
using GaoJD.Club.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace GaoJD.Club.LogstashLogger
{
    public class LogstashLogger<T> : BaseLogger<T>
    {
        public readonly LoggerConsumer loggerConsumer;
        private readonly IOptions<LogstashOption> ioptions;

        public LogstashLogger(IOptions<LogstashOption> ioptions, IConfiguration configuration) : base(configuration)
        {
            this.ioptions = ioptions;

            loggerConsumer = LoggerConsumer.Instance;
            if (!loggerConsumer.IsStart)
            {
                loggerConsumer.Consumer(logs =>
                {
                    //ioptions.Value

                    switch (ioptions.Value.Mode)
                    {
                        case "http":
                            var str = JsonConvert.SerializeObject(logs);
                            HttpUtils.PostJson(ioptions.Value.LogstashUrl, JsonConvert.SerializeObject(logs), null);
                            break;
                        case "tcp":
                            var msglist = logs.Select(log => JsonConvert.SerializeObject(log) + Environment.NewLine);
                            SocketUtils.SendBatchMsg(ioptions.Value.IP, ioptions.Value.Port, msglist);
                            break;
                        default:
                            HttpUtils.PostJson(ioptions.Value.LogstashUrl, JsonConvert.SerializeObject(logs), null);
                            break;
                    }
                });
            }
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
