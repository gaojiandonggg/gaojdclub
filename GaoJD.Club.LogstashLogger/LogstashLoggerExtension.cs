using GaoJD.Club.Core;
using GaoJD.Club.Core.Extensions;
using GaoJD.Club.Core.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.LogstashLogger
{
    public static class LogstashLoggerExtension
    {

        public static void AddLogstashLogger(this IServiceCollection services)
        {
            AddLogstashLogger(services, null);
        }

        public static void AddLogstashLogger(this IServiceCollection services, Action<LogstashOption> action)
        {
            var logstashOption = new LogstashOption();
            if (action != null)
            {
                action(logstashOption);
            }
            else
            {
                IConfiguration configuration = services.GetServiceCollection<IConfiguration>();
                logstashOption = configuration.GetSection("Logger").Get<LogstashOption>() ?? logstashOption;

            }
            Validate(logstashOption);
            services.AddSingleton(p => Options.Create(logstashOption));
            services.AddSingleton(typeof(ILogger<>), typeof(LogstashLogger<>));
        }

        public static void Validate(LogstashOption logstashOption)
        {
            Assert.NotNull(logstashOption.Mode, nameof(logstashOption.Mode));
            switch (logstashOption.Mode)
            {
                case "tcp":
                    Assert.NotNull(logstashOption.LogstashUrl, nameof(logstashOption.LogstashUrl));
                    break;
                case "http":
                    Assert.NotNull(logstashOption.IP, nameof(logstashOption.IP));
                    Assert.NotNull(logstashOption.Port, nameof(logstashOption.Port));
                    break;
            }
        }
    }
}
