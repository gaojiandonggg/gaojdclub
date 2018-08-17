using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interface
{
    public interface IUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        string UserID { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        string LoginName { get; set; }


        void AddUser<T>(T t) where T : class;

    }
}
