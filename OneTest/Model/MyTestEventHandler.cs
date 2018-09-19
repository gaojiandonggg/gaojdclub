using GaoJD.Club.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaoJD.Club.OneTest
{
    public class MyTestEventHandler : IEventHandler<MyTestEventData>
    {
        public void HandleEvent(MyTestEventData eventData)
        {
            Console.WriteLine("one");
        }
    }


    public class MyTestTwoEventHandler : IEventHandler<MyTestEventData>
    {
        public void HandleEvent(MyTestEventData eventData)
        {
            Console.WriteLine("two");
        }
    }
}
