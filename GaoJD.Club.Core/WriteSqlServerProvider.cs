using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core
{
    public class WriteSqlServerProvider : SqlserverProvider
    {
        public WriteSqlServerProvider(string connection) : base(connection)
        {

        }
    }
}
