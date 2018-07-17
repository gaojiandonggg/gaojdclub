using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core
{
    public class ReadSqlServerProvider : SqlserverProvider
    {
        public ReadSqlServerProvider(string connection) : base(connection)
        {

        }
    }
}
