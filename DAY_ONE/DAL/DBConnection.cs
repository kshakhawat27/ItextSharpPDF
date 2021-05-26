using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DAY_ONE.DAL
{
    public class DBConnection
    {
        public static string GetConnection()
        {
            try
            {
                return ConfigurationManager.ConnectionStrings["DBCnnString"].ToString();
            }
            catch (Exception ce)
            {
                throw new ApplicationException("unable to connect" + ce);
            }
        }
    }
}