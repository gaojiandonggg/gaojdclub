using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interface
{
    public interface IRepository<T> where T : class
    {

        int Insert(T t);
    }
}
