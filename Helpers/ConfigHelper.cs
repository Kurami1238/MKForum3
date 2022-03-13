using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MKForum.Helpers
{
    public class ConfigHelper
    {
        private const string _mainDBName = "MainDB";

        //讀取設定檔中的連線字串
        public static string GetConnectionString()
        {
            return GetConnectionString(_mainDBName);
        }

        public static string GetConnectionString(string name)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            return connectionString;
        }

    }
}