using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.Utility
{
    public class ConfigSetting
    {

        private IOptions<AppSettings> _AppSettings;
        public ConfigSetting(IOptions<AppSettings> appSettings)
        {
            _AppSettings = appSettings;
        }

        public AppSettings appSettings
        {
            get
            {
                return _AppSettings.Value;
            }
        }


    }
}
