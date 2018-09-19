using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Aop
{
    public class CustomInterceptor : AbstractInterceptor
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            return context.Invoke(next);
        }
    }
}
