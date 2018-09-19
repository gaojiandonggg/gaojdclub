using GaoJD.Club.Core.Ioc;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaoJD.Club.OneTest.Middleware
{
    public static class UseMiddlewareApplicationBuilderExtensions
    {
        private static bool _isUsed = false;

        public static void UseIocConfiguration(this IApplicationBuilder app)
        {
            if (!_isUsed)
            {
                //配置容器
                app.Use((context, next) =>
                {
                    IocManager.Configure(context.RequestServices);
                    return next();
                });

                _isUsed = true;
            }
        }
    }
}
