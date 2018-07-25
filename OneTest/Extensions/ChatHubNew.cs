using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Logic;
using GaoJD.Club.Redis;
using GaoJD.Club.Utility;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaoJD.Club.OneTest.Extensions
{
    public class ChatHubNew : Hub
    {

        private IRedisClient _redisClient;
        private IUserLogic _userLogic;
        public ChatHubNew(IRedisClient redisClient, IUserLogic userLogic)
        {
            this._redisClient = redisClient;
            this._userLogic = userLogic;
        }


        public override async Task OnConnectedAsync()
        {
            string token = HttpContext.Current.Request.Query["access_token"];
            //groupName 可以从连接请求链接中获取
            string groupName = "222";
            //当前登陆人
            string UserName = token;
            //redis中保存对应组在线列表
            _redisClient.SetHash(groupName, Context.ConnectionId, UserName);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //告诉页面ConnectionId值
            await Clients.Client(this.Context.ConnectionId).SendAsync("getConnectionId", Context.ConnectionId);
            //这里有必要更新组内在线人状态，从redis中获取在线人
            List<string> list = _redisClient.HashGetAll<string>(groupName);

            List<User> listuser = _userLogic.GetAll();


            var item = from c in listuser select new { c.UserName, IsOnLine = list.Contains(c.ID.ToString()) };



            await Clients.Group(groupName).SendAsync("GetALLUserInfo", JsonConvert.SerializeObject(item));

            await base.OnConnectedAsync();
        }



        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string token = HttpContext.Current.Request.Query["access_token"];
            //groupName 可以从连接请求链接中获取
            string groupName = "222";
            //当前登陆人
            string UserName = token;
            //redis中保存对应组在线列表
            _redisClient.RemoveField(groupName, Context.ConnectionId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);


            //这里有必要更新组内在线人状态，从redis中获取在线人
            List<string> list = _redisClient.HashGetAll<string>(groupName);

            // List<User> listuser = _userLogic.GetAll();

            // var item = from c in list select new { c.UserName };

            await Clients.Group(groupName).SendAsync("leftUser", UserName);

            await base.OnDisconnectedAsync(exception);
        }



    }
}
