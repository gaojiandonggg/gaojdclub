using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class IMiddlewareMiddleware : IMiddleware
    {
        private readonly IConfiguration configuration;

        public IMiddlewareMiddleware(IConfiguration configuration)
        {

            this.configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var keyValue = context.Request.Query["key"];

            if (!string.IsNullOrWhiteSpace(keyValue))
            {

            }

            await next(context);
        }
    }
}
