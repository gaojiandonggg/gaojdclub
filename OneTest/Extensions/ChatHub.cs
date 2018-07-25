using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaoJD.Club.OneTest.Model;
using Newtonsoft.Json;
using GaoJD.Club.Utility;
using Microsoft.AspNetCore.Mvc;
using GaoJD.Club.OneTest.Filter;

namespace GaoJD.Club.OneTest.Extensions
{
    public class ChatHub : Hub
    {
        public static UserContext db = new UserContext();


        /// <summary>
        /// 单个发送
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="User"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendClientMessage(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveClientMessage", Context.ConnectionId, message);
        }


        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveClientMessage", Context.ConnectionId, message);
        }

        /// <summary>
        /// 发送消息到组,组下的当前连接的人都能收到
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendMessageToGroup(string groupName, string UserName, string message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveGroupMessage", $"{ UserName}:{ Context.ConnectionId}", message);
        }

        /// <summary>
        /// 加入到组
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public async Task AddToGroup(string groupName, string UserName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", UserName, $"{UserName} has joined the group {groupName}.", Context.ConnectionId);
        }
        /// <summary>
        /// 从某组移除
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public async Task RemoveFromGroup(string groupName, string UserName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", UserName, $"{UserName} has left the group {groupName}.", Context.ConnectionId);
        }

        public override async Task OnConnectedAsync()
        {
            var aa = HttpContext.Current.Request.Cookies;
            // 查询用户。
            var user = db.Users.SingleOrDefault(u => u.UserName == Context.ConnectionId);

            //判断用户是否存在,否则添加
            if (user == null)
            {
                user = new User()
                {
                    UserName = Context.ConnectionId
                };
                db.Users.Add(user);

            }
            //发送房间列表
            var itme = from a in db.Rooms
                       select new { a.RoomName };
            await Clients.Client(this.Context.ConnectionId).SendAsync("getRoomlist", JsonConvert.SerializeObject(itme.ToList()));


            await Clients.Client(this.Context.ConnectionId).SendAsync("getConnectionId", Context.ConnectionId);


            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {

            await Task.Run(() =>
            {
                var user = db.Users.Where(u => u.UserName == Context.ConnectionId).FirstOrDefault();

                //判断用户是否存在,存在则删除
                if (user != null)
                {
                    //删除用户
                    db.Users.Remove(user);
                    // 循环用户的房间,删除用户
                    foreach (var item in user.Rooms)
                    {
                        RemoveFromRoom(item.RoomName);
                    }
                }
                GetRoomList();
            });

            await base.OnDisconnectedAsync(exception);
        }


        /// <summary>
        /// 退出聊天室
        /// </summary>
        /// <param name="roomName"></param>
        public void RemoveFromRoom(string roomName)
        {
            //查找房间是否存在
            var room = db.Rooms.Find(a => a.RoomName == roomName);
            //存在则进入删除
            if (room != null)
            {
                //查找要删除的用户
                var user = room.Users.Where(a => a.UserName == Context.ConnectionId).FirstOrDefault();
                //移除此用户
                room.Users.Remove(user);
                //如果房间人数为0,则删除房间
                if (room.Users.Count <= 0)
                {
                    db.Rooms.Remove(room);
                }



                Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                GetUsersList(roomName);
                //提示客户端
                // Clients.Client(Context.ConnectionId).removeRoom("退出成功!");
            }
        }


        /// <summary>
        /// 创建聊天室
        /// </summary>
        /// <param name="roomName"></param>
        public void CreatRoom(string roomName)
        {
            var room = db.Rooms.Find(a => a.RoomName == roomName);
            if (room == null)
            {
                ConversationRoom cr = new ConversationRoom()
                {
                    RoomName = roomName
                };
                //将房间加入列表
                db.Rooms.Add(cr);
                AddToRoom(roomName);
                GetRoomList();
            }
            else
            {
                Clients.Clients(Context.ConnectionId).SendAsync("alertMsg", "名称已存在");
            }
        }

        /// <summary>
        /// 更新所有用户的房间列表
        /// </summary>
        private void GetRoomList()
        {
            var itme = from a in db.Rooms
                       select new { a.RoomName };
            string jsondata = JsonConvert.SerializeObject(itme.ToList());
            Clients.All.SendAsync("getRoomlist", jsondata);
        }


        /// <summary>
        /// 更新所有用户的房间列表
        /// </summary>
        private void GetUsersList(string Room)
        {
            var room = db.Rooms.Where(p => p.RoomName == Room).SingleOrDefault();
            if (room != null)
            {
                var users = room?.Users;
                var item = from a in users
                           select new { a.UserName };
                string jsondata = JsonConvert.SerializeObject(item.ToList());
                Clients.Group(Room).SendAsync("getUserlist", jsondata);
            }
            else
            {
                Clients.Group(Room).SendAsync("getUserlist", null);
            }
        }


        /// <summary>
        /// 加入聊天室
        /// </summary>
        /// <param name="roomName"></param>
        public void AddToRoom(string roomName)
        {
            //查询聊天室
            var room = db.Rooms.Find(a => a.RoomName == roomName);
            //存在则加入
            if (room != null)
            {
                //查找房间中是否存在此用户
                var isuser = room.Users.Where(a => a.UserName == Context.ConnectionId).FirstOrDefault();
                //不存在则加入
                if (isuser == null)
                {
                    var user = db.Users.Find(a => a.UserName == Context.ConnectionId);
                    user.Rooms.Add(room);
                    room.Users.Add(user);
                    Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                    GetUsersList(roomName);
                }
                else
                {
                    Clients.Clients(Context.ConnectionId).SendAsync("alertMsg", "已在聊天室当中");
                }

            }
        }

    }
}
