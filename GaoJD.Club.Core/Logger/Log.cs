using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core.Logger
{
    public class Log
    {
        public string Type { get; set; }

        public string Opbusinessline { get; set; }

        public int ApplicationID { get; set; }

        public string Application { get; set; }

        public string Message { get; set; }

        public DateTime CreateTime { get; set; }

        public IDictionary<string, object> Extras { get; set; }
    }
}
