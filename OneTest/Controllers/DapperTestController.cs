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

        [HttpGet]
        public string Test(Func<string, string> func)
        {
            return func("5");
        }

    }
}
