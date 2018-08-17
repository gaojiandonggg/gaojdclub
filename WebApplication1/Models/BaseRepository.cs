using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Interface;

namespace WebApplication1.Models
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly ConnectionStringResolverBase connectionStringResolverBase;

        public BaseRepository():base()
        {

        }

        public BaseRepository(ConnectionStringResolverBase connectionStringResolverBase)
        {
            this.connectionStringResolverBase = connectionStringResolverBase;
        }


        public int Insert(T t)
        {
            return 1;
        }
    }
}
