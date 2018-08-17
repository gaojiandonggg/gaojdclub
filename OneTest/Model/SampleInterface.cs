using GaoJD.Club.Utility.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaoJD.Club.OneTest.Model
{
    [SampleInterceptor]
    public interface SampleInterface
    {
        void Foo();
    }
}
