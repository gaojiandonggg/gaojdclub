using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Logger
{
    public static class AddLoggerExtensions
    {
        public static void AddLogger(this IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
        }
    }
}
