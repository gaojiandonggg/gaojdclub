
using GaoJD.Club.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaoJD.Club.OneTest
{
    public class Platform
    {

    }

    public class DataBaseProvider
    {
        private AppSettings dataBaserProvider = new AppConfigurtaionServices().GetAppSettings<AppSettings>("appsettings");

        public bool _isSqlServer
        {
            get
            {
                return dataBaserProvider.DataBase.ToLower() == "mssql";
            }

        }



        public bool _isMySql
        {
            get
            {
                return dataBaserProvider.DataBase.ToLower() == "mysql";
            }
        }



        public bool _isOracle
        {
            get
            {
                return dataBaserProvider.DataBase.ToLower() == "oracle";
            }
        }


    }
}
