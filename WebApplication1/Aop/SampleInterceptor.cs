using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Aop
{
    public class SampleInterceptor : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            //方法返回值为 void 不会代理
            if (context.ImplementationMethod.ReturnType == typeof(void))
            {
                // await next(context);
                await next(context);
            }
            else
            {
                context.ReturnValue = "11";
            }
        }
    }
}
