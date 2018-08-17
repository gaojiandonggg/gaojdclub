using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Interface;

namespace WebApplication1.Models
{
    public class User : IUser
    {

        public virtual string UserID { get; set; }
        public virtual string LoginName { get; set; }



        public void AddUser<T>(T t) where T : class
        {

        }
    }
}
