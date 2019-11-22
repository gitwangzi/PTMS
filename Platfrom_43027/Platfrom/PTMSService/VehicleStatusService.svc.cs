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
///// Create Time: 2013/8/17 15:02:53
///// Class Description:  
/////======================================================================
/////  Modified Time: 
/////  Modified by: 
/////  Modified Description: 
/////======================================================================


using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.SecuritySuite.Contract;
using Gsafety.PTMS.SecuritySuite.Contract.Data;
using Gsafety.PTMS.SecuritySuite.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
//using System.Data.Services;
//using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using System.Diagnostics;
using Gsafety.PTMS.DBEntity;

namespace Gs.PTMS.Service
{
    public class VehicleStatusService : BaseService, IVehicleStatusService
    {
        private VehicleStatusRepository dbHelper;
        public VehicleStatusService()
        {
            dbHelper = new VehicleStatusRepository();
        }

        public MultiMessage<VehicleStatus> GetVehicleStatusByDistrict(string districtCode)
        {
            try
            {
                Info("GetVehicleStatusByDistrict");
                Info("districtCode:" + Convert.ToString(districtCode));

                var temp = new List<VehicleStatus>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetVehicleStatusByDistrict(context, districtCode);
                }

                MultiMessage<VehicleStatus> result = new MultiMessage<VehicleStatus> { Result = temp };
                Log<VehicleStatus>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<VehicleStatus>() { ExceptionMessage = exp };
            }
        }

        public MultiMessage<VehicleStatus> GetVehicleStatusByDistrictEx(string districtCode, int vehicleType)
        {
            try
            {
                Info("GetVehicleStatusByDistrictEx");
                Info("districtCode:" + Convert.ToString(districtCode) + ";" + "vehicleType:" + Convert.ToString(vehicleType));

                var temp = new List<VehicleStatus>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    //temp = dbHelper.GetVehicleStatusByDistrictEx(context, districtCode, vehicleType);
                }

                MultiMessage<VehicleStatus> result = new MultiMessage<VehicleStatus> { Result = temp };
                Log<VehicleStatus>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<VehicleStatus>() { ExceptionMessage = exp };
            }
        }

        public MultiMessage<VehicleStatus> GetVehicleStatusByGroup(string groupId)
        {
            try
            {
                Info("GetVehicleStatusByGroup");
                Info("groupId:" + Convert.ToString(groupId));

                var temp = new List<VehicleStatus>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetVehicleStatusByGroup(context, groupId);
                }

                MultiMessage<VehicleStatus> result = new MultiMessage<VehicleStatus> { Result = temp };
                Log<VehicleStatus>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<VehicleStatus>() { ExceptionMessage = exp };
            }
        }

        public MultiMessage<VehicleStatus> GetVehicleStatusByStatus(string districtCode, bool isOnline)
        {
            try
            {
                Info("GetVehicleStatusByStatus");
                Info("districtCode:" + Convert.ToString(districtCode) + ";" + "isOnline:" + Convert.ToString(isOnline));
                var temp = new List<VehicleStatus>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetVehicleStatusByStatus(context, districtCode, isOnline);
                }

                MultiMessage<VehicleStatus> result = new MultiMessage<VehicleStatus> { Result = temp };
                Log<VehicleStatus>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<VehicleStatus>() { ExceptionMessage = exp };
            }
        }
        //GetVehicleStatusByVehicleNumber
        public SingleMessage<VehicleStatus> GetVehicleStatusByVehicleNumber(string vehicleId)
        {
            try
            {
                Info("GetVehicleStatusByVehicleNumber");
                Info("vehicleId:" + Convert.ToString(vehicleId));
                var temp = new VehicleStatus();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetVehicleStatusByVehicleNumber(context, vehicleId);
                }

                SingleMessage<VehicleStatus> result = new SingleMessage<VehicleStatus> { Result = temp };
                Log<VehicleStatus>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new SingleMessage<VehicleStatus>() { ExceptionMessage = exp };
            }
        }
        //GetVehicleTimeSpan
        public MultiMessage<SuiteStatus> GetVehicleTimeSpanFuzzy(string vehicleId, string suiteId, int onlineStatus, string timespan, PagingInfo pageInfo)
        {
            try
            {
                Info("GetVehicleTimeSpanFuzzy");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "onlineStatus:" + Convert.ToString(onlineStatus) + ";" + "timespan:" + Convert.ToString(timespan) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                var temp = new MultiMessage<SuiteStatus>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetVehicleTimeSpanFuzzy(context, vehicleId, suiteId, onlineStatus, timespan, pageInfo);
                }

                MultiMessage<SuiteStatus> result = new MultiMessage<SuiteStatus> { Result = temp.Result, TotalRecord = temp.TotalRecord, IsSuccess = true };
                Log<SuiteStatus>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<SuiteStatus>() { ExceptionMessage = ex, TotalRecord = 0 };
            }
        }
        //GetVehicleStatusByTimeSpan
        public MultiMessage<VehicleStatus> GetVehicleStatusByTimeSpan(int timespan)
        {
            try
            {
                Info("GetVehicleStatusByTimeSpan");
                Info("timespan:" + Convert.ToString(timespan));
                var temp = new List<VehicleStatus>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetVehicleStatusByTimeSpan(context, timespan);
                }

                MultiMessage<VehicleStatus> result = new MultiMessage<VehicleStatus> { Result = temp };
                Log<VehicleStatus>(result);
                return result;
            }
            catch (Exception exp)
            {
                Error(exp);
                return new MultiMessage<VehicleStatus>() { ExceptionMessage = exp };
            }
        }
        //GetSuiteStatusFuzzy
        public MultiMessage<SuiteStatus> GetSuiteStatusFuzzy(string vehicleId, string suiteId, int onlineStatus, int timespan, PagingInfo pageInfo)
        {
            try
            {
                Info("GetSuiteStatusFuzzy");
                Info("vehicleId:" + Convert.ToString(vehicleId) + ";" + "suiteId:" + Convert.ToString(suiteId) + ";" + "onlineStatus:" + Convert.ToString(onlineStatus) + ";" + "timespan:" + Convert.ToString(timespan) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                UserInfoMessageHeader userInfo = GetUserInfo();
                MultiMessage<SuiteStatus> result = new MultiMessage<SuiteStatus>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = dbHelper.GetSuiteStatusFuzzy(context, userInfo, vehicleId, suiteId, onlineStatus, timespan, pageInfo);
                }

                Log<SuiteStatus>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<SuiteStatus>() { ExceptionMessage = ex, TotalRecord = 0 };
            }
        }

        //SuiteStatusMangement
        public MultiMessage<InitialSuiteMangement> GetSuiteStatusManagement(string suiteId, int currentStatus, PagingInfo pageInfo)
        {
            try
            {
                Info("GetSuiteStatusManagement");
                Info("suiteId:" + Convert.ToString(suiteId) + ";" + "currentStatus:" + Convert.ToString(currentStatus) + ";" + "pageInfo:" + Convert.ToString(pageInfo));
                Stopwatch t = new Stopwatch();
                t.Start();
                UserInfoMessageHeader userInfo = GetUserInfo();
                var temp = new MultiMessage<InitialSuiteMangement>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.GetSuiteStatusManagement(context, userInfo, suiteId, currentStatus, pageInfo);
                }

                t.Stop();
                MultiMessage<InitialSuiteMangement> result = new MultiMessage<InitialSuiteMangement> { Result = temp.Result, IsSuccess = true, TotalRecord = temp.TotalRecord };
                Log<InitialSuiteMangement>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<InitialSuiteMangement>() { ExceptionMessage = ex, TotalRecord = 0 };
            }
        }
        public bool RunningToAbnoraml(SuiteMangementInfo newSuiteStatus)
        {
            try
            {
                Info("RunningToAbnoraml");
                Info("newSuiteStatus:" + Convert.ToString(newSuiteStatus));
                bool result = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = dbHelper.RunningToAbnoraml(context, newSuiteStatus);
                }

                Info(result.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
            }
            return true;
        }
        public bool AbnoramlToRepair(SuiteMangementInfo newSuiteStatus)
        {
            try
            {
                Info("AbnoramlToRepair");
                Info("newSuiteStatus:" + Convert.ToString(newSuiteStatus));
                bool result = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = dbHelper.AbnoramlToRepair(context, newSuiteStatus);
                }

                Info(result.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
            }
            return true;
        }
        public bool AbnoramlToRunning(SuiteMangementInfo newSuiteStatus)
        {
            try
            {
                Info("AbnoramlToRunning");
                Info("newSuiteStatus:" + Convert.ToString(newSuiteStatus));

                bool result = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = dbHelper.AbnoramlToRunning(context, newSuiteStatus);
                }

                Info(result.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
            }
            return true;
        }
        public bool RepairToInitial(SuiteMangementInfo newSuiteStatus)
        {
            try
            {
                Info("RepairToInitial");
                Info("newSuiteStatus:" + Convert.ToString(newSuiteStatus));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = dbHelper.RepairToInitial(context, newSuiteStatus);
                }

                Info(temp.ToString());
                return temp;
            }
            catch (Exception ex)
            {
                Error(ex);
            }
            return true;
        }
    }
}
