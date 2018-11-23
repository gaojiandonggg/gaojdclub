using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GateWayServiceOne.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OneController : Controller
    {

        public static int count = 0;

      
        [HttpGet]
        public string GetOne()
        {
            return "我是第一个";
        }

        [HttpGet]
        public string GetQosOne()
        {
            if (count < 3)
            {
                Thread.Sleep(5000);
            }
            count++;
            return "我是第一个";
        }
    }
}