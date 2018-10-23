using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.LogstashLogger
{

    public class LogstashOption
    {
        /// <summary>
		/// http or tcp
		/// </summary>
		public string Mode { get; set; }

        #region http
        /// <summary>
        /// logstash http url
        /// </summary>
        public string LogstashUrl { get; set; }
        #endregion

        #region tcp
        /// <summary>
        /// tcp IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// tcp Port
        /// </summary>
        public int Port { get; set; }
        #endregion
    }
}
