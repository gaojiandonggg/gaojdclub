using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Interface;

namespace WebApplication1.Models
{
    public class SecondUser : ISession<User>
    {

        public virtual string Token { get; set; }

        public virtual bool IsAuthenticated { get; set; }

        public virtual User TUser { get; set; }

    }
}
