using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Core.Exceptions
{
    public class ExceptionInfo
    {
        public string Type { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.AppendFormat(" [type]: {0}.", this.Type);
            buf.AppendFormat(" [source]: {0}.", this.Source);
            buf.AppendFormat(" [message]: {0}.", this.Message);
            buf.AppendFormat(" [stacktrace]: {0}.", this.StackTrace);
            return buf.ToString();
        }
    }
}
