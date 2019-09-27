using Gsafety.Common.Logging;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 21e6da41-9c53-475e-a9e2-6a64779e0b12      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement
/////    Project Description:    
/////             Class Name: OracleHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/28 17:10:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/28 17:10:27
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.CommandManagement
{
    public static class OracleHelper
    {
        /// <summary>
        /// connectionstring
        /// </summary>
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;

        /// <summary>
        /// execute sql
        /// </summary>
        /// <param name="sql"></param>
        public static void ExecuteSql(string sql)
        {
            try
            {
                using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
                {
                    oracleConnection.Open();
                    using (OracleCommand cmd = new OracleCommand(sql, oracleConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
    }
}
