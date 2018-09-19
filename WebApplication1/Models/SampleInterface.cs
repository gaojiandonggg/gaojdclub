using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Aop;

namespace WebApplication1.Models
{
    [SampleInterceptor]
    public interface SampleInterface
    {
        void Foo();
    }
}
