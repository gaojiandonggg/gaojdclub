using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GaoJD.Club.OneTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DapperTestController : ControllerBase
    {
        // GET: api/DapperTest
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        public void SetCookid()
        {
            GaoJD.Club.Utility.HttpContext.Current.Response.Cookies.Append("woshiceshi", "2117");
        }

        [HttpGet]
        public void GetCookid()
        {
            string aaa = Utility.HttpContext.Current.Request.Cookies["woshiceshi"]?.ToString();
        }

        [HttpPost]
        public string Add()
        {
            var connection = new SqlConnection(OpenConfiguration.Configuration["ConnectionStrings:ReadSqlServerConnection"]);

            var sql = "select * from Users where Email in @emails";
            var info = connection.Query<Users>(sql, new { emails = new string[2] { "5qq.com", "7qq.com" } });


            sql = "select * from Users;select * from Product;";
            var multiReader = connection.QueryMultiple(sql);
            var productList = multiReader.Read<Product>();
            var userList = multiReader.Read<Users>();



            //string sql1 = SqlSugor.GetWhereByLambda<Users>(x => x.UserName.StartsWith("test") && x.UserID > 2);
            // sql1 = SqlSugor.GetWhereByLambda<Users>(x => x.UserName.EndsWith("test") && (x.UserID > 4 || x.UserID == 3));
            //sql1 = SqlSugor.GetWhereByLambda<Users>(x => x.UserName.Contains("test") && (x.UserID > 4 && x.UserID <= 8));
            //sql1 = SqlSugor.GetWhereByLambda<Users>(x => x.UserName == "FengCode" && x.UserID >= 1);

            //sql1 = SqlSugor.GetWhereByLambda<Users>(x => x.UserName == "aaa");
            //sql1 = SqlSugor.GetWhereByLambda<Users>(x => x.UserName == "aaa" && x.Email == "aaaa");
            //sql1 = SqlSugor.GetWhereByLambda<Users>(x => x.UserName.Equals("aaa"));


            //sql1 = SqlSugor.GetWhereByLambda<Users>(x => x.UserName.Equals("aaa"));
            //sql1 = SqlSugor.GetWhereByLambda<Users>(x => x.UserName != "bbbb");

            //sql1 = SqlSugor.GetWhereByLambda<Users>(x => x.UserName.Trim() != "bbbb");





            //Func<string, string> func = p => "[" + p + "]";
            //var aaa = func("5");

            //var bbb = Test(func);


            return "";
        }

        /// <summary>
        /// 获取 Guid
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        private Guid GetGuid()   //与Func<Guid> 兼容
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 异步获取 Guid
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task GetGuidAsync()
        {
            var myFunc = new Func<Guid>(GetGuid);
            var t1 = await Task.Run(myFunc);

            var t2 = await Task.Run(new Func<Guid>(GetGuid));

            var t3 = await Task.Run(() => GetGuid());

            var t4 = await Task.Run(() => Guid.NewGuid());

            Console.WriteLine($"t1: {t1}");
            Console.WriteLine($"t2: {t2}");
            Console.WriteLine($"t3: {t3}");
            Console.WriteLine($"t4: {t4}");


        }


        [HttpGet]
        public string Test(Func<string, string> func)
        {
            return func("5");
        }

    }
}
