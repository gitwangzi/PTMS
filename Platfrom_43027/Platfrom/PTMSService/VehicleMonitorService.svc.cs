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
using Gsafety.PTMS.Common.Data;

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
namespace Gs.PTMS.Service
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
        public MultiMessage<VehicleAlert> GetVehicleAlert(string vehicleId, DateTime startDate, DateTime endDate, string alertType)
        {
            try
            {
                Info("GetVehicleAlert");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "startDate:" + Convert.ToString(startDate) + ";" + "endDate:" + Convert.ToString(endDate) + ";" + "alertType:" + Convert.ToString(alertType));
                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = dbHelper.GetVehicleAlert(context, vehicleId, startDate, endDate, alertType);
                    MultiMessage<VehicleAlert> result = new MultiMessage<VehicleAlert> { Result = temp };
                    Log<VehicleAlert>(result);
                    return result;
                }
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<VehicleAlert>() { ExceptionMessage = exp };
            }
        }

        public MultiMessage<GPS> GetMonitorGPSTrack(string vehicleId, DateTime startTime, DateTime endTime)
        {
            try
            {
                Info("GetMonitorGPSTrack");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "startTime:" + Convert.ToString(startTime) + ";" + "endTime:" + Convert.ToString(endTime));
                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = dbHelper.GetMonitorGPSTrack(context, vehicleId, startTime, endTime);
                    MultiMessage<GPS> result = new MultiMessage<GPS> { Result = temp };
                    Log<GPS>(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<GPS>() { ExceptionMessage = ex };
            }
        }

        public SingleMessage<GPS> GetLastMonitorGPS(string vehicleId)
        {
            try
            {
                Info("GetLastMonitorGPS");
                Info("vehicleId:" + Convert.ToString(vehicleId));
                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = dbHelper.GetVehicleLastGPS(context, vehicleId);
                    SingleMessage<GPS> result = new SingleMessage<GPS> { Result = temp };
                    Log<GPS>(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<GPS>() { ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> ValidateSuiteGPS(string vehicleId, string mdvrCoreSn)
        {

            try
            {
                Info("GetLastMonitorGPS");
                Info("vehicleId:" + Convert.ToString(vehicleId));
                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = dbHelper.ValidateSuiteGPS(context, vehicleId, mdvrCoreSn);
                    SingleMessage<bool> result = new SingleMessage<bool> { Result = temp };
                    Log<bool>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> ValidateGPSGPS(string vehicleId, string gpsUid)
        {
            try
            {
                Info("GetLastMonitorGPS");
                Info("vehicleId:" + Convert.ToString(vehicleId));
                using (PTMSEntities context = new PTMSEntities())
                {
                    var temp = dbHelper.ValidateGPSGPS(context, vehicleId, gpsUid);
                    SingleMessage<bool> result = new SingleMessage<bool> { Result = temp };
                    Log<bool>(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex };
            }
        }


        #endregion
    }
}
