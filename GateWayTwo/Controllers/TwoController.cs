using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GateWayServiceTwo.Controllers
{

    [Route("api/[controller]/[action]")]
    public class TwoController : Controller
    {
    
        [HttpGet]
        public string GetTwo()
        {
            return "我是第二个";
        }
    }
}