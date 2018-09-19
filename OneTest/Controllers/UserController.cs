using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneTest;
using OneTest.Controllers;
using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Logic;
using GaoJD.Club.Utility;
using GaoJD.Club.Logger;
using GaoJD.Club.Redis;
using System.Data.SqlClient;
using System.Data;
using GaoJD.Club.Dto;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using GaoJD.Club.Utility.Common;
using Microsoft.Extensions.DependencyInjection;
using GaoJD.Club.Core;
using System.Net.Http;

namespace GaoJD.Club.OneTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : BaseApiController
    {

        int bbb = 4;
        int aaa = 5;
        int ccc = 6;
        int ddd = 7;
        int eee = 8;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IUserLogic _UserLogic;
        AppConfigurtaionServices _AppConfigurtaionServices;
        private ILogger _Logger;
        private IHttpContextAccessor _accessor;
        private readonly SampleInterface sampleInterface;

        public UserController(IHttpClientFactory clientFactory, IUserLogic UserLogic, AppConfigurtaionServices AppConfigurtaionServices, ILogger Logger, IHttpContextAccessor accessor, IRedisClient redisClient, SampleInterface sampleInterface) : base(redisClient)
        {
            this._clientFactory = clientFactory;
            _UserLogic = UserLogic;
            this._AppConfigurtaionServices = AppConfigurtaionServices;
            this._Logger = Logger;
            this._accessor = accessor;
            this.sampleInterface = sampleInterface;
        }

        [HttpGet]
        public string TestReturnString()
        {
            return "xxxxxxxxxxxxxxxxx";
        }

        [HttpGet]
        public async Task TestBaidu()
        {


            for (int i = 0; i < 14; i++)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://www.baidu.com");
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
            }


        }

        [HttpGet]
        public void TestAop()
        {
            sampleInterface.Foo();
        }

        [HttpGet]
        public void EventBusTest()
        {

            EventBus.Default.Trigger(new MyTestEventData() { Name = 1 });
            EventBus.Default.Trigger(typeof(MyTestEventHandler), new MyTestEventData() { Name = 1 });

            EventBus.Default.Register<MyTestEventData>(p => { p.Name = 111; });
            EventBus.Default.Trigger(new MyTestEventData() { Name = 1 });
        }


        [HttpPost]
        public ActionResult<User> InsetUser([FromBody]User usr)
        {
            //  throw new Exception("你好");
            //throw new ApplicationException("你好");
            throw new OperationFailedException("你好");


            try
            {

                //_accessor.HttpContext.Session.SetString("aaa", "bbb");
                //HttpContext.Session.SetString("aaa", "bbb");
                _UserLogic.Insert(usr);
                var httpcontext = _accessor.HttpContext;
                _Logger.AddOperate<User>("测试", usr);
                Console.WriteLine($" {usr} fefefef {User} ");
                Console.WriteLine($" {usr} fefefef {User} " + "aaa");
                return usr;
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                return usr;
            }
        }


        /// <summary>
        /// 通过主键获取
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<User> GetByID(int ID)
        {
            User usr = _UserLogic.GetById(ID);
            return usr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<User> GetUser(string userName, string userPwd)
        {
            User usr = _UserLogic.GetUser(userName, userPwd);
            return usr;
        }


        /// <summary>
        /// 通过主键获取
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            int total = 0;
            long num = _UserLogic.GetCount();
            num = _UserLogic.GetCountByQuery(p => p.UserName == "string");

            var usrList = _UserLogic.GetAll();
            var usr = _UserLogic.GetItemByQuery(p => p.ID == 5);
            usrList = _UserLogic.GetListByQuery(p => p.UserName == "string" && p.Password == "string");
            usrList = _UserLogic.GetListByQuery(p => p.UserName == "string", true, p => p.Password);
            usrList = _UserLogic.GetListByQuery(p => p.UserName == "string", true, p => p.ID);

            usrList = _UserLogic.GetPagedList(1, 10, out total);
            usrList = _UserLogic.GetPagedList(1, 10, out total, false, p => p.UserName == "string", p => p.ID);



            usrList = _UserLogic.ExecuteStoredProcedureQuery("Pr_User_GetAll");


            SqlParameter[] param = new SqlParameter[] {
                 new SqlParameter("@UserName",SqlDbType.NVarChar),
                 new SqlParameter("@PassWord",SqlDbType.NVarChar),
            };
            param[0].Value = "string";
            param[1].Value = "string";
            usrList = _UserLogic.ExecuteStoredProcedureQuery("Pr_User_GetAll_ByUserName", param);


            usrList = _UserLogic.ExecuteSqlQuery("select * from  [User]");

            usrList = _UserLogic.ExecuteSqlQuery("select * from  [User]  where UserName=@UserName and PassWord=@PassWord", param);

            int aaa = _UserLogic.ExecuteSql(@"INSERT INTO  dbo.[User]
            (UserName, Password)
    VALUES('1', --UserName - nvarchar(50)
              '2'-- Password - nvarchar(50)
              )");

            aaa = _UserLogic.ExecuteSql(@"INSERT INTO  dbo.[User]
            (UserName, Password)

    VALUES(@UserName, --UserName - nvarchar(50)

              @PassWord-- Password - nvarchar(50)
              )", param);

            aaa = _UserLogic.ExecuteStoredProcedure("Pr_User_Insert");

            aaa = _UserLogic.ExecuteStoredProcedure("Pr_User_InsertByUserName", param);


            return usrList;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public void RemoveByID(int id)
        {
            _UserLogic.Delete(id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public void RemoveByEntity([FromBody]User usr)
        {
            _UserLogic.Delete(usr);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public void RemoveBylamda()
        {
            _UserLogic.Delete(p => p.UserName == "22");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usr"></param>
        [HttpPost]
        public void Edit([FromBody]User usr)
        {
            _UserLogic.Update(usr);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="usr"></param>
        [HttpPost]
        public void AddList()
        {
            List<User> list = new List<User>() {
                 new User{UserName="高建东"+Math.Round((double)10),Password="1111" },
                   new User{UserName="高建东"+Math.Round((double)10),Password="1111" },
                     new User{UserName="高建东"+Math.Round((double)10),Password="1111" }
            };
            _UserLogic.AddList(list);
        }


        [HttpPost]
        public string PostInput([FromBody]User_Input user_Input)
        {
            Type t1 = user_Input.GetType();
            PropertyInfo propertyInfo = t1.GetProperty("Email");
            propertyInfo.SetValue(user_Input, "1111");

            return "我是测试";
        }



        [HttpGet]
        public string GetInput([Required(ErrorMessage = "我是测试")]string Name)
        {
            return "我是测试";
        }
    }
}
