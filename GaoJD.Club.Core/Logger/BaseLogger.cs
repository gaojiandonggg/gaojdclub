using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GaoJD.Club.Core.Logger
{
    public abstract class BaseLogger<T> : ILogger<T>
    {
        public static AsyncLocal<Dictionary<string, object>> asyncLocal = new AsyncLocal<Dictionary<string, object>>();
        private readonly IConfiguration configuration;

        public BaseLogger(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string LoggerName => typeof(T).FullName;

        public ILogger<T> AddExtras(string key, object value)
        {
            if (asyncLocal.Value == null)
            {
                asyncLocal.Value = new Dictionary<string, object>();
            }
            if (!asyncLocal.Value.ContainsKey(key))
            {
                asyncLocal.Value.Add(key, value);
            }
            return this;
        }

        public abstract void Log(string Type, string message);


        public abstract void LogError(Exception ex);



        public abstract void LogError(string message, Exception ex);



        public abstract void LogInfo(string message);

        public void ClearExtras()
        {
            asyncLocal.Value?.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Log GetLog(string Type, string message)
        {

            this.AddExtras("1111", "2222");


            var log = new Log();
            log.Type = Type;
            log.Message = message;
            log.Opbusinessline = configuration.GetValue<string>("Application:OpBusinessLine") ?? "default";
            log.ApplicationID = configuration.GetValue<int>("Application:ApplicationID");
            log.Application = configuration.GetValue<string>("Application:ApplicationName") ?? "default";

            log.CreateTime = DateTime.Now;
            if (asyncLocal.Value != null)
            {
                log.Extras = asyncLocal.Value;
            }
            return log;
        }

    }
}
