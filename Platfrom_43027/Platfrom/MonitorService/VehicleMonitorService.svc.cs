using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.Common.Util;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Monitor.Contract.Data;
using Gsafety.PTMS.Monitor.Contract;
using Gsafety.PTMS.Monitor.Repository;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
///// Guid: a2a104b0-cf3f-43b2-b2c8-eefd726551f5      
///// clrversion: 4.0.30319.17929
/////Registered organization: 
///// Machine Name: PC-LANQ
///// Author: Hongsheng Shi
/////======================================================================
/////  Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////  Project Description:    
///// Class Name: MonitorRepository
///// Class Version: v1.0.0.0
///// Create Time: 2013/8/19 15:47:53
///// Class Description:  
/////======================================================================
/////  Modified Time: 2013/8/23 15:47:53
/////  Modified by: 
/////  Modified Description: 
/////======================================================================
namespace Gsafety.PTMS.Monitor.Service
{
    [ServiceKnownType(typeof(VehicleGPS))]
    public class VehicleMonitorService : BaseService, IVehicleMonitorService
    {
        MonitorRepository dbHelper;

        public VehicleMonitorService()
        {
            dbHelper = new MonitorRepository();
        }

        #region IVehicleMonitorService
        /// <summary>
        /// AddOnline  for  Test
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public SingleMessage<bool> AddOnline(OnOffline item)
        {
            PTMSEntities context = new PTMSEntities();

            try
            {
                Info("AddOnline");
                Info("item:" + Convert.ToString(item));
                bool b = false;

                //dbHelper.AddOnlineTest(context, item);


                SingleMessage<bool> result = new SingleMessage<bool> { Result = b };
                Log<bool>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new SingleMessage<bool>() { ExceptionMessage = exp, Result = false };

            }

        }
        public MultiMessage<VehicleAlert> GetVehicleAlert(string vehicleId, DateTime startDate, DateTime endDate, string alertType)
        {

            PTMSEntities context = new PTMSEntities();

            try
            {
                Info("GetVehicleAlert");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "startDate:" + Convert.ToString(startDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "alertType:" + Convert.ToString(alertType));
                var temp = dbHelper.GetVehicleAlert(context, vehicleId, startDate, endDate, alertType);
                MultiMessage<VehicleAlert> result = new MultiMessage<VehicleAlert> { Result = temp };
                Log<VehicleAlert>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<VehicleAlert>() { ExceptionMessage = exp };
            }

        }

        public MultiMessage<Gsafety.PTMS.Alarm.Contract.Data.GPS> GetMonitorGPSTrack(string vehicleId, DateTime startTime, DateTime endTime)
        {

            PTMSEntities context = new PTMSEntities();

            try
            {
                Info("GetMonitorGPSTrack");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime));
                var temp = dbHelper.GetMonitorGPSTrack(context, vehicleId, startTime, endTime);
                MultiMessage<Gsafety.PTMS.Alarm.Contract.Data.GPS> result = new MultiMessage<Gsafety.PTMS.Alarm.Contract.Data.GPS> { Result = temp };
                Log<Gsafety.PTMS.Alarm.Contract.Data.GPS>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Gsafety.PTMS.Alarm.Contract.Data.GPS>() { ExceptionMessage = ex };
            }

        }

        public SingleMessage<PTMSGPS> GetLastMonitorGPS(string vehicleId)
        {
            PTMSEntities context = new PTMSEntities();
            try
            {
                Info("GetLastMonitorGPS");
                Info("vehicleId:" + Convert.ToString(vehicleId));
                var temp = dbHelper.GetVehicleLastGPS(context, vehicleId);
                SingleMessage<PTMSGPS> result = new SingleMessage<PTMSGPS> { Result = temp };
                Log<PTMSGPS>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<PTMSGPS>() { ExceptionMessage = ex };
            }
        }
        #endregion
    }
}
