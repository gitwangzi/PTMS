/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 40ce2767-2b58-48e1-9cfe-c5e3e0f78cb9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Repository
/////    Project Description:    
/////             Class Name: AlarmGpsRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/24 10:28:45
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/24 10:28:45
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Monitor.Contract.Data;
using Gsafety.PTMS.Message.Contract.Data;
using System.Data.OracleClient;

namespace Gsafety.PTMS.Alarm.Repository
{
    public static class AlarmGpsRepository
    {
        private static PTMSEntities _context = new PTMSEntities();

        static AlarmGpsRepository()
        {
            ////close ef model validate           
            _context.Configuration.AutoDetectChangesEnabled = false;
            _context.Configuration.ValidateOnSaveEnabled = false;
            _context.ALARM_LOCATION.AsNoTracking();
            _context.Set<ALARM_LOCATION>().AsNoTracking();
            _context.ChangeTracker.DetectChanges();
            _context.ChangeTracker.Entries<ALARM_LOCATION>();
        }

        /// <summary>
        /// add alarm GPS
        /// </summary>
        /// <param name="model"></param>
        public static void AddAlarmGPS(GPS model)
        {
            ALARM_LOCATION entity = new ALARM_LOCATION();
            entity.ID = Guid.NewGuid().ToString();
            entity.LONGITUDE = model.Longitude;
            entity.LATITUDE = model.Latitude;
            entity.DIRECTION = model.Direction;
            entity.SPEED = model.Speed;
            entity.GPS_VALID = model.GPSValid;
            entity.GPS_TIME = model.GPSTime;
            entity.MDVR_CORE_SN = model.MdvrCoreId;
            entity.SUITE_INFO_ID = model.SuiteInfoId;
            entity.VEHICLE_ID = model.VehicleId;
            entity.DISTRICT_CODE = model.DistrictCode;
            _context.ALARM_LOCATION.Add(entity);
        }

        public static void SaveChanges()
        {
            _context.SaveChanges();
        }

        public static void ExecuteSqlCommand(string sql)
        {
            _context.Database.ExecuteSqlCommand(sql);
        }
    }
}
