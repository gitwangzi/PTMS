/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f1e13d01-8fe5-42e5-83e6-0e3209c8ceec      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Storage
/////    Project Description:    
/////             Class Name: AlarmGps
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/22 16:08:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 16:08:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using System.Collections;
using Gsafety.Common.Util;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.Analysis.Storage
{
    public static class AlarmGpsStorage
    {

        private static StringBuilder _sbSql = new StringBuilder();

        /// <summary>
        ///Alarming data access
        /// </summary>
        /// <param name="model"></param>
        public static bool AddSuite(byte[] bytes)
        {
            bool isSuccessAdd = false;
            try
            {
                //LoggerManager.Logger.Info("AddSuite.......");
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                LoggerManager.Logger.Info(str);
                //json -> entity
                GpsInfo gpsinfo = JsonHelper.FromJsonString<Gsafety.PTMS.Common.Data.GpsInfo>(str);

                if (gpsinfo != null)
                {
                    //LoggerManager.Logger.Info("gpsinfo is not empty");
                    GPS gps = GetGPS(gpsinfo);

                    gps.Source = (short)GPSSourceEnum.Suite;


                    if (CacheManager.Suites.ContainsKey(gpsinfo.UID))
                    {
                        //LoggerManager.Logger.Info("found suite in cache");
                        SuiteWorking suite = CacheManager.Suites[gpsinfo.UID];
                        gps.VehicleId = suite.VehicleID;
                        gps.ClientID = suite.ClientID;
                        gps.Source = (short)GPSSourceEnum.Suite;

                        if (CacheManager.District.ContainsKey(gps.VehicleId))
                        {
                            gps.DistrictCode = CacheManager.District[gps.VehicleId];
                        }
                    }
                    LoggerManager.Logger.Info(gps.InsertSQL());
                    _sbSql.Append(gps.InsertSQL());
                    isSuccessAdd = true;
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Converted gps to entity is empty,string:{0}", str));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
            return isSuccessAdd;
        }

        private static GPS GetGPS(GpsInfo gpsinfo)
        {
            GPS gps = new GPS();
            gps.AlarmFlag = gpsinfo.AlarmFlag;
            gps.Direction = gpsinfo.Direction;
            gps.Status = gpsinfo.Status;
            gps.SourceMode = 0;
            try
            {
                gps.GpsTime = DateTime.Parse(gpsinfo.GpsTime);
            }
            catch (Exception)
            {

            }

            gps.Valid = gpsinfo.Valid;
            gps.Height = gpsinfo.Height;
            gps.Latitude = gpsinfo.Latitude;
            gps.Longitude = gpsinfo.Longitude;
            gps.Speed = gpsinfo.Speed;
            gps.UID = gpsinfo.UID;
            gps.DeviceID = gpsinfo.UID;
            return gps;
        }

        /// <summary>
        ///Alarming data access
        /// </summary>
        /// <param name="model"></param>
        public static bool AddGPS(byte[] bytes)
        {
            bool isSuccessAdd = false;
            try
            {
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                LoggerManager.Logger.Info(str);
                //json -> entity
                Gsafety.PTMS.Common.Data.GpsInfo gpsinfo = JsonHelper.FromJsonString<Gsafety.PTMS.Common.Data.GpsInfo>(str);

                if (gpsinfo != null)
                {
                    GPS gps = GetGPS(gpsinfo);
                    gps.Source = (short)GPSSourceEnum.GPS;
                    if (CacheManager.GPSs.ContainsKey(gpsinfo.UID))
                    {
                        GPSWorking gpsworking = CacheManager.GPSs[gpsinfo.UID];
                        gps.VehicleId = gpsworking.VehicleID;
                        gps.ClientID = gpsworking.ClientID;
                        gps.Source = (short)GPSSourceEnum.GPS;

                        if (CacheManager.District.ContainsKey(gps.VehicleId))
                        {
                            gps.DistrictCode = CacheManager.District[gps.VehicleId];
                        }
                        LoggerManager.Logger.Info(gps.InsertSQL());
                        _sbSql.Append(gps.InsertSQL());
                        isSuccessAdd = true;
                    }
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Converted gps to entity is empty,string:{0}", str));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
            return isSuccessAdd;
        }

        /// <summary>
        ///Alarming data access
        /// </summary>
        /// <param name="model"></param>
        public static bool AddMobile(byte[] bytes)
        {
            bool isSuccessAdd = false;
            try
            {
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                LoggerManager.Logger.Info(str);
                //json -> entity
                Gsafety.PTMS.Common.Data.GpsInfo gpsinfo = JsonHelper.FromJsonString<Gsafety.PTMS.Common.Data.GpsInfo>(str);


                if (gpsinfo != null)
                {
                    GPS gps = GetGPS(gpsinfo);
                    string[] fields = gps.UID.Split(";".ToCharArray());
                    gps.UID = fields[0];
                    gps.DeviceID = gps.UID;
                    gps.Source = (short)GPSSourceEnum.Mobile;

                    if (CacheManager.Mobiles.ContainsKey(gps.UID))
                    {
                        MobileWorking mobile = CacheManager.Mobiles[gps.UID];
                        gps.VehicleId = mobile.VehicleID;
                        gps.Source = (short)GPSSourceEnum.Mobile;
                        gps.ClientID = mobile.ClientID;

                        if (CacheManager.District.ContainsKey(gps.VehicleId))
                        {
                            gps.DistrictCode = CacheManager.District[gps.VehicleId];
                        }
                        LoggerManager.Logger.Info(gps.InsertSQL());
                        _sbSql.Append(gps.InsertSQL());
                        isSuccessAdd = true;
                    }
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Converted gps to entity is empty,string:{0}", str));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
            return isSuccessAdd;
        }

        /// <summary>
        /// Save
        /// </summary>
        public static void Save()
        {
            try
            {
                if (_sbSql.Length > 0)
                {
                    _sbSql.Insert(0, " BEGIN ");
                    _sbSql.Append(" END; ");

                    //OracleHelper.ExecuteSql(_sbSql.ToString());
                    //AlarmGpsRepository.ExecuteSqlCommand(_sbSql.ToString());
                    OracleHelper.ExecuteSql(_sbSql.ToString());
                }

                //AlarmGpsRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
            finally
            {
                _sbSql.Clear();
            }
        }
    }
}
