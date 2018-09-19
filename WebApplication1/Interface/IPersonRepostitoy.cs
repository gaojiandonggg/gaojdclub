using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Aop;
using WebApplication1.Models;

namespace WebApplication1.Interface
{
    public interface IPersonRepostitoy : IRepository<Person>
    {


        void GetPerson(string UserName);

        [SampleInterceptor]
        string GetPerson1(string UserName1);
    }
}
