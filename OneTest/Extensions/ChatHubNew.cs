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

        public class UserInfo
        {
            public string UserName { get; set; }
            public string groupName { get; set; }
        }

        public override async Task OnConnectedAsync()
        {
            string token = HttpContext.Current.Request.Query["access_token"];
            UserInfo info = JsonConvert.DeserializeObject<UserInfo>(token);
            //groupName 可以从连接请求链接中获取
            string groupName = info.groupName;
            //当前登陆人
            string UserName = info.UserName;
            //redis中保存对应组在线列表
            _redisClient.SetHash(groupName, UserName, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //告诉页面ConnectionId值
            await Clients.Client(this.Context.ConnectionId).SendAsync("getConnectionId", Context.ConnectionId);
            //这里有必要更新组内在线人状态，从redis中获取在线人
            List<string> list = _redisClient.HashGetAllKey<string>(groupName);
            List<User> listuser = _userLogic.GetAll();
            var item = from c in listuser select new { c.UserName, c.ID, IsOnLine = list.Contains(c.ID.ToString()) };
            await Clients.Group(groupName).SendAsync("GetALLUserInfo", JsonConvert.SerializeObject(item));

            await base.OnConnectedAsync();
        }



        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string token = HttpContext.Current.Request.Query["access_token"];
            UserInfo info = JsonConvert.DeserializeObject<UserInfo>(token);
            //groupName 可以从连接请求链接中获取
            string groupName = info.groupName;
            //当前登陆人
            string UserName = info.UserName;
            _redisClient.RemoveField(groupName, UserName);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            //这里有必要更新组内在线人状态，从redis中获取在线人
            List<string> list = _redisClient.HashGetAllKey<string>(groupName);
            List<User> listuser = _userLogic.GetAll();
            var item = from c in listuser select new { c.UserName, c.ID, IsOnLine = list.Contains(c.ID.ToString()) };
            await Clients.Group(groupName).SendAsync("GetALLUserInfo", JsonConvert.SerializeObject(item));
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 发送消息到组,组下的当前连接的人都能收到
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendMessageToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveGroupMessage", Context.ConnectionId, message);
        }

        public Task SendMessageToUser(string message)
        {
            string userid = HttpContext.Current.Request.Cookies["woshiceshi"]?.ToString();
            return Clients.User(userid).SendAsync("SendMessageToUser", message);
        }


    }
}
