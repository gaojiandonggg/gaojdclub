using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Interface;

namespace WebApplication1.Models
{
    public class PersonRepostitoy : BaseRepository<Person>, IPersonRepostitoy
    {
        public PersonRepostitoy()
        {

        }

        public void GetPerson(string UserName)
        {
            throw new NotImplementedException();
        }

        public string GetPerson1(string UserName1)
        {
            return "";
        }
    }
}
