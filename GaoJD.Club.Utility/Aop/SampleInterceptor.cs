using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GaoJD.Club.Utility.Aop
{
    public class SampleInterceptor : AbstractInterceptorAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            Console.WriteLine("call interceptor");
            return context.Invoke(next);
        }
    }
}
