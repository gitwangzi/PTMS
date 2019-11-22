using Gsafety.Common.Logging;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Message.Contract.Data;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5d566358-367a-417d-84c6-8347e4eb9d2b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Storage
/////    Project Description:    
/////             Class Name: OracleHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/27 17:38:55
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/27 17:38:55
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Gsafety.PTMS.Analysis.Storage
{

    public static class OracleHelper
    {
        /// <summary>
        /// connectionstring
        /// </summary>
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;


        /// <summary>
        /// execute sql
        /// </summary>
        /// <param name="sql"></param>
        public static void ExecuteSql(string sql)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, sqlConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// query
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<SimpleSuiteInfo> ExecuteSqlWithCache()
        {
            List<SimpleSuiteInfo> list = new List<SimpleSuiteInfo>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();

                    UpdateSuiteCache(sqlConnection);

                    UpdateGPSCache(sqlConnection);

                    UpdateMobileCache(sqlConnection);

                    UpdateDistrictCache(sqlConnection);

                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            return list;
        }

        private static void UpdateDistrictCache(SqlConnection sqlConnection)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;
            command.CommandText = "Select VEHICLE_ID,DISTRICT_CODE From BSC_VEHICLE where VEHICLE_ID in (select VEHICLE_ID from RUN_SUITE_WORKING) or VEHICLE_ID in (select VEHICLE_ID from RUN_GPS_WORKING) or VEHICLE_ID in (select VEHICLE_ID from RUN_MOBILE_WORKING)";
            SqlDataReader reader = command.ExecuteReader();
            lock (CacheManager.District)
            {
                while (reader.Read())
                {
                    if (!CacheManager.District.ContainsKey(reader.GetString(reader.GetOrdinal("VEHICLE_ID"))))
                    {
                        CacheManager.District.Add(reader.GetString(reader.GetOrdinal("VEHICLE_ID")), reader.GetString(reader.GetOrdinal("DISTRICT_CODE")));
                    }
                }
            }

            reader.Close();
        }

        private static void UpdateMobileCache(SqlConnection sqlConnection)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;
            command.CommandText = "Select * From RUN_MOBILE_WORKING";
            SqlDataReader reader = command.ExecuteReader();
            lock (CacheManager.Mobiles)
            {
                while (reader.Read())
                {
                    MobileWorking item = GetMobileFromReader(reader);

                    CacheManager.Mobiles.Add(item.MobileNumber, item);
                }
            }

            reader.Close();
        }

        private static void UpdateGPSCache(SqlConnection sqlConnection)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;
            command.CommandText = "Select * From RUN_GPS_WORKING";
            SqlDataReader reader = command.ExecuteReader();
            lock (CacheManager.GPSs)
            {
                while (reader.Read())
                {
                    GPSWorking item = GetGPSSuiteFromReader(reader);

                    CacheManager.GPSs.Add(item.GPSSN, item);
                }
            }

            reader.Close();
        }

        private static void UpdateSuiteCache(SqlConnection sqlConnection)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;

            command.CommandText = "Select * From RUN_SUITE_WORKING";
            SqlDataReader reader = command.ExecuteReader();
            lock (CacheManager.Suites)
            {
                while (reader.Read())
                {
                    SuiteWorking item = GetWorkingSuiteFromReader(reader);

                    CacheManager.Suites.Add(item.MdvrCoreSn, item);
                }
            }

            reader.Close();
        }

        private static MobileWorking GetMobileFromReader(SqlDataReader reader)
        {
            MobileWorking item = new MobileWorking();
            item.ClientID = reader.GetString(reader.GetOrdinal("CLIENT_ID"));
            item.MobileNumber = reader.GetString(reader.GetOrdinal("MOBILE_NUMBER"));
            item.OnlineFlag = reader.GetInt32(reader.GetOrdinal("ONLINE_FLAG"));
            item.OrganizationID = reader.GetString(reader.GetOrdinal("ORGANIZATION_ID"));
            if (!reader.IsDBNull(reader.GetOrdinal("SWITCH_TIME")))
                item.SwitchTime = reader.GetDateTime(reader.GetOrdinal("SWITCH_TIME"));
            item.VehicleID = reader.GetString(reader.GetOrdinal("VEHICLE_ID"));
            return item;
        }

        private static GPSWorking GetGPSSuiteFromReader(SqlDataReader reader)
        {
            GPSWorking item = new GPSWorking();
            item.GPSID = reader.GetString(reader.GetOrdinal("GPS_ID"));
            item.ClientID = reader.GetString(reader.GetOrdinal("CLIENT_ID"));
            if (!reader.IsDBNull(reader.GetOrdinal("VEHICLE_ID")))
                item.VehicleID = reader.GetString(reader.GetOrdinal("VEHICLE_ID"));
            if (!reader.IsDBNull(reader.GetOrdinal("ONLINE_FLAG")))
                item.OnlineFlag = reader.GetInt32(reader.GetOrdinal("ONLINE_FLAG"));
            if (!reader.IsDBNull(reader.GetOrdinal("SWITCH_TIME")))
                item.SwitchTime = reader.GetDateTime(reader.GetOrdinal("SWITCH_TIME"));
            if (!reader.IsDBNull(reader.GetOrdinal("STATUS")))
                item.Status = reader.GetInt32(reader.GetOrdinal("STATUS"));
            if (!reader.IsDBNull(reader.GetOrdinal("FAULT_TIME")))
                item.FaultTime = reader.GetDateTime(reader.GetOrdinal("FAULT_TIME"));
            if (!reader.IsDBNull(reader.GetOrdinal("ABNORMAL_CAUSE")))
                item.AbnormalCause = reader.GetString(reader.GetOrdinal("ABNORMAL_CAUSE"));
            if (!reader.IsDBNull(reader.GetOrdinal("ORGANIZATION_ID")))
                item.OrganizationID = reader.GetString(reader.GetOrdinal("ORGANIZATION_ID"));
            if (!reader.IsDBNull(reader.GetOrdinal("GPS_SN")))
                item.GPSSN = reader.GetString(reader.GetOrdinal("GPS_SN"));
            return item;
        }

        private static SuiteWorking GetWorkingSuiteFromReader(SqlDataReader reader)
        {
            SuiteWorking item = new SuiteWorking();
            item.SuiteInfoID = reader.GetString(reader.GetOrdinal("SUITE_INFO_ID"));
            item.ClientID = reader.GetString(reader.GetOrdinal("CLIENT_ID"));
            if (!reader.IsDBNull(reader.GetOrdinal("VEHICLE_ID")))
                item.VehicleID = reader.GetString(reader.GetOrdinal("VEHICLE_ID"));
            if (!reader.IsDBNull(reader.GetOrdinal("MDVR_CORE_SN")))
                item.MdvrCoreSn = reader.GetString(reader.GetOrdinal("MDVR_CORE_SN"));
            if (!reader.IsDBNull(reader.GetOrdinal("STATUS")))
                item.Status = reader.GetInt32(reader.GetOrdinal("STATUS"));
            if (!reader.IsDBNull(reader.GetOrdinal("SWITCH_TIME")))
                item.SwitchTime = reader.GetDateTime(reader.GetOrdinal("SWITCH_TIME"));
            if (!reader.IsDBNull(reader.GetOrdinal("ONLINE_FLAG")))
                item.OnlineFlag = reader.GetInt32(reader.GetOrdinal("ONLINE_FLAG"));
            if (!reader.IsDBNull(reader.GetOrdinal("FAULT_TIME")))
                item.FaultTime = reader.GetDateTime(reader.GetOrdinal("FAULT_TIME"));
            if (!reader.IsDBNull(reader.GetOrdinal("ABNORMAL_CAUSE")))
                item.AbnormalCause = reader.GetString(reader.GetOrdinal("ABNORMAL_CAUSE"));
            if (!reader.IsDBNull(reader.GetOrdinal("ORGANIZATION_ID")))
                item.OrganizationID = reader.GetString(reader.GetOrdinal("ORGANIZATION_ID"));
            return item;
        }

        public static void ProcessSuiteInstall(string mdvrCoreSn)
        {
            LoggerManager.Logger.Info("ProcessSuiteInstall:" + mdvrCoreSn);
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = sqlConnection;

                command.CommandText = "Select * From RUN_SUITE_WORKING where MDVR_CORE_SN='" + mdvrCoreSn.Trim() + "'";
                var reader = command.ExecuteReader();
                string vehicle = string.Empty;
                if (reader.Read())
                {
                    SuiteWorking item = GetWorkingSuiteFromReader(reader);
                    lock (CacheManager.Suites)
                    {
                        if (CacheManager.Suites.ContainsKey(mdvrCoreSn))
                        {
                            CacheManager.Suites.Remove(mdvrCoreSn);
                        }
                        CacheManager.Suites.Add(item.MdvrCoreSn, item);
                    }

                    vehicle = item.VehicleID;
                }

                reader.Close();

                if (!CacheManager.District.ContainsKey(vehicle))
                {
                    command.CommandText = "Select DISTRICT_CODE From BSC_VEHICLE where VEHICLE_ID='" + vehicle.Trim() + "'";
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        CacheManager.District.Add(vehicle, reader.GetString(reader.GetOrdinal("DISTRICT_CODE")));
                    }

                    reader.Close();
                }
            }
        }

        public static void ProcessGPSInstall(string gpsid)
        {
            LoggerManager.Logger.Info("ProcessGPSInstall:" + gpsid);
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = sqlConnection;

                command.CommandText = "Select * From RUN_GPS_WORKING where GPS_SN='" + gpsid.Trim() + "'";
                var reader = command.ExecuteReader();
                string vehicle = string.Empty;
                if (reader.Read())
                {
                    GPSWorking item = GetGPSSuiteFromReader(reader);

                    lock (CacheManager.GPSs)
                    {
                        if (CacheManager.GPSs.ContainsKey(gpsid))
                        {
                            CacheManager.GPSs.Remove(gpsid);
                        }
                        CacheManager.GPSs.Add(item.GPSSN, item);
                    }
                    vehicle = item.VehicleID;
                }

                reader.Close();

                if (!CacheManager.District.ContainsKey(vehicle))
                {
                    command.CommandText = "Select DISTRICT_CODE From BSC_VEHICLE where VEHICLE_ID='" + vehicle.Trim() + "'";
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        CacheManager.District.Add(vehicle, reader.GetString(reader.GetOrdinal("DISTRICT_CODE")));
                    }

                    reader.Close();
                }
            }
        }

        public static void ProcessMobileInstall(string sim)
        {
            LoggerManager.Logger.Info("ProcessMobileInstall:" + sim);
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = sqlConnection;

                command.CommandText = "Select * From RUN_MOBILE_WORKING where MOBILE_NUMBER='" + sim.Trim() + "'";
                var reader = command.ExecuteReader();
                string vehicle = string.Empty;
                if (reader.Read())
                {
                    MobileWorking item = GetMobileFromReader(reader);

                    lock (CacheManager.Mobiles)
                    {
                        if (CacheManager.Mobiles.ContainsKey(sim))
                        {
                            CacheManager.Mobiles.Remove(sim);
                        }
                        CacheManager.Mobiles.Add(item.MobileNumber, item);
                    }

                    vehicle = item.VehicleID;
                }

                reader.Close();

                if (!CacheManager.District.ContainsKey(vehicle))
                {
                    command.CommandText = "Select DISTRICT_CODE From BSC_VEHICLE where VEHICLE_ID='" + vehicle.Trim() + "'";
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        CacheManager.District.Add(vehicle, reader.GetString(reader.GetOrdinal("DISTRICT_CODE")));
                    }

                    reader.Close();
                }
            }
        }

        public static void ProcessSuiteUnInstall(string mdvrCoreSn)
        {
            lock (CacheManager.Suites)
            {
                if (CacheManager.Suites.ContainsKey(mdvrCoreSn))
                    CacheManager.Suites.Remove(mdvrCoreSn);
            }
        }

        public static void ProcessGPSUnInstall(string gpsid)
        {
            lock (CacheManager.GPSs)
            {
                if (CacheManager.GPSs.ContainsKey(gpsid))
                    CacheManager.GPSs.Remove(gpsid);
            }
        }

        public static void ProcessMobileUnInstall(string sim)
        {
            lock (CacheManager.Mobiles)
            {
                CacheManager.Mobiles.Remove(sim);
            }
        }
    }
}
