using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class DefaultConnectionStringResolver : ConnectionStringResolverBase
    {
        public override string ResolveReadConnectionString()
        {
            throw new NotImplementedException();
        }

        public override string ResolveWriteConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}
