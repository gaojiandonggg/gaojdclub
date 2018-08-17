using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interface
{
    public interface ISession<T> where T : IUser
    {
        string Token { get; }

        bool IsAuthenticated { get; }

        T TUser { get; }
    }
}
