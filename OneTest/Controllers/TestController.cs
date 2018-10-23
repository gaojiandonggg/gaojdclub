using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Core;
using GaoJD.Club.Core.Utility;
using GaoJD.Club.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GaoJD.Club.OneTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        static AsyncLocal<string> _asyncLocalString = new AsyncLocal<string>();

        static ThreadLocal<string> _threadLocalString = new ThreadLocal<string>();

        static async Task AsyncMethodA()
        {
            // Start multiple async method calls, with different AsyncLocal values.
            // We also set ThreadLocal values, to demonstrate how the two mechanisms differ.
            _asyncLocalString.Value = "Value 1";
            _threadLocalString.Value = "Value 1";
            await AsyncMethodB("Value 1");


            _asyncLocalString.Value = "Value 2";
            _threadLocalString.Value = "Value 2";
            await AsyncMethodB("Value 2");

        }

        static async Task AsyncMethodB(string expectedValue)
        {
            Console.WriteLine("Entering AsyncMethodB.");
            Console.WriteLine("   Expected '{0}', AsyncLocal value is '{1}', ThreadLocal value is '{2}'",
                              expectedValue, _asyncLocalString.Value, _threadLocalString.Value);
            await Task.Delay(100);
            Console.WriteLine("Exiting AsyncMethodB.");
            Console.WriteLine("   Expected '{0}', got '{1}', ThreadLocal value is '{2}'",
                              expectedValue, _asyncLocalString.Value, _threadLocalString.Value);

            _asyncLocalString.Value = "Value 6";
        }
        [HttpGet]
        public void Test()
        {
            AsyncMethodA().Wait();


        }

        [HttpGet]
        public void TestMapper()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 100; i++)
            {
                var user = new User();
                user.ID = 78;
                user.Password = "111";
                MapperPropertyInfo[] mappers = SqlServerUtilities.GetMapperPropertyInfo<User>(user);

                var user1 = new User();
                MapperPropertyInfo[] mappers1 = SqlServerUtilities.GetMapperPropertyInfo<User>(null);

                var users3 = new Users();
                MapperPropertyInfo[] mapperss3 = SqlServerUtilities.GetMapperPropertyInfo<Users>(null);
            }
            sw.Stop();
            long aaa = sw.ElapsedMilliseconds;

        }

        [HttpGet]
        public void TestStatic()
        {

        }


    }
}