﻿using GaoJD.Club.Utility.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaoJD.Club.OneTest
{
    [SampleInterceptor]
    public interface SampleInterface
    {
        void Foo();
    }
}
