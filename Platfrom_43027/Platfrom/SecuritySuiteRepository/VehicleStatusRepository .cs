/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 387d6431-5f45-409a-9466-6139e9c72d5d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Repository
/////    Project Description:    
/////             Class Name: VehicleStatusHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/4 18:10:58
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/4 18:10:58
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.SecuritySuite.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;




namespace Gsafety.PTMS.SecuritySuite.Repository
{

    public class VehicleStatusRepository : BaseRepository
    {
        private int newtime;
        public int StatusTimeSpan
        {
            get { return newtime; }
            set
            {
                newtime = value;
            }
        }
        /// <summary>
        ///  Get  vehicles status by districtCode
        /// </summary>
        /// <returns>The list of vehicles status</returns>
        public List<VehicleStatus> GetVehicleStatusByDistrict(PTMSEntities context, string districtCode)
        {
            //var result = from f in context.SECURITY_SUITE_WORKING
            //             join v in context.VEHICLE
            //             on f.VEHICLE_ID equals v.VEHICLE_ID

            //             where v.DISTRICT_CODE == districtCode
            //             select new VehicleStatus
            //             {
            //                 CarNumber = f.VEHICLE_ID,
            //                 AbnormalCause = f.ABNORMAL_CAUSE,
            //                 IsOnline = f.ONLINE_FLAG ?? 0,
            //                 CarType = v.VEHICLE_TYPE,
            //                 MdvrCoreId = f.MDVR_CORE_SN,
            //                 Status = f.STATUS,
            //                 SutieInfoId = f.SUITE_INFO_ID.Trim()
            //             };
            //return result.ToList();

            return null;
        }

        /// <summary>
        ///  Get  vehicles status by districtCode
        /// </summary>
        /// <param name="vehicleType"> Vehicle Type</param>
        /// <returns>The list of vehicles status</returns>
        public List<VehicleStatus> GetVehicleStatusByDistrictEx(PTMSEntities context, string districtCode, string vehicleType)
        {

            //var result = from f in context.SECURITY_SUITE_WORKING
            //             join v in context.VEHICLE
            //             on f.VEHICLE_ID equals v.VEHICLE_ID
            //             where v.DISTRICT_CODE == districtCode
            //             && v.VEHICLE_TYPE == vehicleType
            //             select new VehicleStatus
            //             {
            //                 CarNumber = f.VEHICLE_ID,
            //                 AbnormalCause = f.ABNORMAL_CAUSE,
            //                 IsOnline = f.ONLINE_FLAG ?? 0,                                // 1 is online ,0 is offline 
            //                 CarType = v.VEHICLE_TYPE,
            //                 MdvrCoreId = f.MDVR_CORE_SN,
            //                 Status = f.STATUS,
            //                 SutieInfoId = f.SUITE_INFO_ID
            //             };
            //return result.ToList();
            return null;

        }


        /// <summary>
        /// GetVehicleStatusByGroup 
        /// </summary>
        /// <returns>The list of vehicles status</returns>
        public List<VehicleStatus> GetVehicleStatusByGroup(PTMSEntities context, string groupId)
        {
        //    var result = from f in context.SECURITY_SUITE_WORKING
        //                 join v in context.VEHICLE
        //                 on f.VEHICLE_ID equals v.VEHICLE_ID
        //                 select new VehicleStatus
        //                 {
        //                     CarNumber = f.VEHICLE_ID,
        //                     AbnormalCause = f.ABNORMAL_CAUSE,
        //                     IsOnline = f.ONLINE_FLAG ?? 0,
        //                     CarType = v.VEHICLE_TYPE,
        //                     MdvrCoreId = f.MDVR_CORE_SN,
        //                     Status = f.STATUS,
        //                     SutieInfoId = f.SUITE_INFO_ID
        //                 };
        //    return result.ToList();
            return null;
        }

        /// <summary>
        ///GetVehicleStatusByVehicleNumber
        /// </summary>
        /// <param name="vehicleId">Vehicle ID</param>
        /// <returns>The list of vehicles status</returns>
        public VehicleStatus GetVehicleStatusByVehicleNumber(PTMSEntities context, string vehicleId)
        {

            //var result = from f in context.SECURITY_SUITE_WORKING
            //             join v in context.VEHICLE
            //             on f.VEHICLE_ID equals v.VEHICLE_ID
            //             where
            //                 f.VEHICLE_ID == vehicleId
            //             select new VehicleStatus
            //             {
            //                 CarNumber = f.VEHICLE_ID,
            //                 AbnormalCause = f.ABNORMAL_CAUSE,
            //                 IsOnline = f.ONLINE_FLAG ?? 0,
            //                 CarType = v.VEHICLE_TYPE,
            //                 MdvrCoreId = f.MDVR_CORE_SN,
            //                 Status = f.STATUS,
            //                 SutieInfoId = f.SUITE_INFO_ID
            //             };
            //return result.FirstOrDefault();
            return null;
        }
        /// <summary>
        /// GetVehicleStatusByTimeSpan
        /// </summary>
        /// <param name="districtCode"></param>
        /// <param name="isOnline"></param>
        /// <returns></returns>
        public List<VehicleStatus> GetVehicleStatusByTimeSpan(PTMSEntities context, int timespan)
        {
            //var result = from f in context.SECURITY_SUITE_WORKING
            //             join v in context.VEHICLE
            //             on f.VEHICLE_ID equals v.VEHICLE_ID
            //             where (f.SWITCH_TIME.Value.AddHours(timespan) < DateTime.Now) && (f.ONLINE_FLAG == 1)
            //             select new VehicleStatus
            //             {
            //                 CarNumber = f.VEHICLE_ID,
            //                 AbnormalCause = f.ABNORMAL_CAUSE,
            //                 IsOnline = f.ONLINE_FLAG ?? 0,
            //                 CarType = v.VEHICLE_TYPE,
            //                 MdvrCoreId = f.MDVR_CORE_SN,
            //                 Status = f.STATUS,
            //                 SutieInfoId = f.SUITE_INFO_ID
            //             };
            //return result.ToList();
            return null;
        }
        public List<VehicleStatus> GetVehicleStatusByStatus(PTMSEntities context, string districtCode, bool isOnline)
        {

            //var result = from f in context.SECURITY_SUITE_WORKING
            //             join v in context.VEHICLE
            //             on f.VEHICLE_ID equals v.VEHICLE_ID
            //             where f.ONLINE_FLAG == (isOnline ? (short)1 : (short)0)
            //             && v.DISTRICT_CODE == districtCode
            //             select new VehicleStatus
            //             {
            //                 CarNumber = f.VEHICLE_ID,
            //                 AbnormalCause = f.ABNORMAL_CAUSE,
            //                 IsOnline = f.ONLINE_FLAG ?? 0,
            //                 CarType = v.VEHICLE_TYPE,
            //                 MdvrCoreId = f.MDVR_CORE_SN,
            //                 Status = f.STATUS,
            //                 SutieInfoId = f.SUITE_INFO_ID
            //             };
            //return result.ToList();
            return null;
        }
        /// <summary>
        /// SuiteStaus 
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="suiteId"></param>
        /// <param name="onlineStatus"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public MultiMessage<SuiteStatus> GetSuiteStatusFuzzy(PTMSEntities context, UserInfoMessageHeader userInfo, string vehicleId, string suiteId, int onlineStatus, int timespan, PagingInfo pageInfo)
        {

            MultiMessage<SuiteStatus> result = new MultiMessage<SuiteStatus>();
            DateTime time = DateTime.Now.AddHours(-timespan);
            if (vehicleId != null || timespan != 0)
            {
                var source = from working in context.VEHICLE_WORKING_VIEW.Where(item => item.ONLINE_FLAG == onlineStatus)
                             join detail in context.MTN_INSTALLATION_DETAIL.Where(item => item.VALID == 1)
                             on working.VEHICLE_ID equals detail.VEHICLE_ID
                             where (string.IsNullOrEmpty(vehicleId) ? true : working.VEHICLE_ID.ToLower().Contains(vehicleId.Trim().ToLower())) && working.SWITCH_TIME <= time
                             select new SuiteStatus
                             {
                                 VehicleID = working.VEHICLE_ID,
                                 //VehicleType = working.VEHICLE_TYPE,
                                 Owner = working.OWNER,
                                 //OwnerPhone = working.OWNER_PHONE,
                                 StatusChangeTime = working.SWITCH_TIME,
                             };
                if (pageInfo.PageIndex > 0)
                {
                    result.TotalRecord = source.Count();
                    result.Result = source.OrderBy(v => v.StatusChangeTime).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                }
                else
                {
                    result.TotalRecord = source.Count();
                    result.Result = source.ToList();
                }
            }
            else
            {
                var source = from working in context.VEHICLE_WORKING_VIEW
                             where (string.IsNullOrEmpty(vehicleId) ? true : working.VEHICLE_ID.ToLower().Contains(vehicleId.Trim().ToLower()))
                             && working.ONLINE_FLAG == onlineStatus
                             select new SuiteStatus
                             {
                                 VehicleID = working.VEHICLE_ID,
                                 //VehicleType = working.VEHICLE_TYPE,
                                 Owner = working.OWNER,
                                 //OwnerPhone = working.OWNER_PHONE,
                                 StatusChangeTime = working.SWITCH_TIME,
                             };

                if (pageInfo.PageIndex > 0)
                {
                    result.TotalRecord = source.Count();
                    result.Result = source.OrderBy(v => v.StatusChangeTime).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                }
                else
                {
                    result.TotalRecord = source.Count();
                    result.Result = source.ToList();
                }
            }
            foreach (var item in result.Result)
            {
                item.StatusTimeSpan = (TimeSpan)(DateTime.Now - (item.StatusChangeTime == null ? DateTime.Now : item.StatusChangeTime));
            }
            return result;

        }
        public MultiMessage<SuiteStatus> GetVehicleTimeSpanFuzzy(PTMSEntities context, string vehicleId, string suiteId, int onlineStatus, string timespan, PagingInfo pageInfo)
        {

            MultiMessage<SuiteStatus> result = new MultiMessage<SuiteStatus>();
            if (timespan == "")
            { timespan = null; }
            int timespan1 = Convert.ToInt32(timespan);
            DateTime time = DateTime.Now.AddHours(-timespan1);
            if (vehicleId != null || timespan != null)
            {
                var source = from working in context.VEHICLE_WORKING_VIEW.Where(item => item.ONLINE_FLAG == onlineStatus)
                             join detail in context.MTN_INSTALLATION_DETAIL.Where(item => item.VALID == 1 && item.CHECKSTEP >= 7) on working.VEHICLE_ID equals detail.VEHICLE_ID
                             join setupStation in context.BSC_SETUP_STATION.Where(item => item.VALID == 1) on detail.STATION_ID equals setupStation.ID
                             where (string.IsNullOrEmpty(vehicleId) ? true : working.VEHICLE_ID.ToLower().Contains(vehicleId.Trim().ToLower())) && working.SWITCH_TIME <= time
                             select new SuiteStatus
                             {
                                 VehicleID = working.VEHICLE_ID,
                                 //VehicleType = working.VEHICLE_TYPE,
                                 Owner = working.OWNER,
                                 //OwnerPhone = working.OWNER_PHONE,
                                 StatusChangeTime = working.SWITCH_TIME,
                             };
                if (pageInfo.PageIndex > 0)
                {
                    result.TotalRecord = source.Count();
                    result.Result = source.OrderBy(v => v.StatusChangeTime).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                }
                else
                {
                    result.TotalRecord = source.Count();
                    result.Result = source.ToList();
                }
            }
            else
            {
                var source = from working in context.VEHICLE_WORKING_VIEW
                             join detail in context.MTN_INSTALLATION_DETAIL.Where(item => item.VALID == 1 && item.CHECKSTEP >= 7) on working.VEHICLE_ID equals detail.VEHICLE_ID
                             join setupStation in context.BSC_SETUP_STATION.Where(item => item.VALID == 1) on detail.STATION_ID equals setupStation.ID
                             where (string.IsNullOrEmpty(vehicleId) ? true : working.VEHICLE_ID.ToLower().Contains(vehicleId.Trim().ToLower()))
                             && working.ONLINE_FLAG == onlineStatus
                             select new SuiteStatus
                             {
                                 VehicleID = working.VEHICLE_ID,
                                 //VehicleType = working.VEHICLE_TYPE,
                                 Owner = working.OWNER,
                                 //OwnerPhone = working.OWNER_PHONE,
                                 StatusChangeTime = working.SWITCH_TIME,
                             };

                if (pageInfo.PageIndex > 0)
                {
                    result.TotalRecord = source.Count();
                    result.Result = source.OrderBy(v => v.StatusChangeTime).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                }
                else
                {
                    result.TotalRecord = source.Count();
                    result.Result = source.ToList();
                }
            }
            foreach (var item in result.Result)
            {
                item.StatusTimeSpan = (TimeSpan)(DateTime.Now - (item.StatusChangeTime == null ? DateTime.Now : item.StatusChangeTime));
            }
            return result;

        }
        //SuiteStatusManagement
        //10:Inital   22：testing  23: running 24:abnormal 25:waiting repair 30:repair 40:crap 99:history

        public MultiMessage<InitialSuiteMangement> GetSuiteStatusManagement(PTMSEntities context, UserInfoMessageHeader userInfo, string suiteId, int currentStatus, PagingInfo pageInfo)
        {
            MultiMessage<InitialSuiteMangement> result = new MultiMessage<InitialSuiteMangement>();

            if (String.IsNullOrEmpty(suiteId) && currentStatus == 0)
            {
                var source = (from suiteInfo in context.STATUS_CHANGING_VIEW
                              select new InitialSuiteMangement
                               {
                                   SuiteID = suiteInfo.SUITE_ID,
                                   VehicleID = suiteInfo.VEHICLE_ID,
                                   CurrentStatus = suiteInfo.STATUS_WORKING,
                                   TempCurrentStatus = suiteInfo.STATUS,
                                   MdvrCoreId = suiteInfo.MDVR_CORE_SN,
                                   SuiteINFOID = suiteInfo.SUITE_INFO_ID,
                               }).ToList();
                System.Threading.Tasks.Parallel.ForEach(source, item =>
                {
                    if (item.CurrentStatus == null)
                        item.CurrentStatus = item.TempCurrentStatus;
                });
                if (pageInfo.PageIndex > 0)
                {
                    result.TotalRecord = source.Count();
                    result.Result = source.OrderBy(v => v.SuiteID).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                }
                else
                {
                    result.TotalRecord = source.Count();
                    result.Result = source.ToList();
                }
            }

            else
            {

                if (!String.IsNullOrEmpty(suiteId) && currentStatus == 0)
                {
                    var source = (from suiteInfo in context.STATUS_CHANGING_VIEW
                                  where suiteInfo.SUITE_ID.ToLower().Contains(suiteId.Trim().ToLower())
                                  select new InitialSuiteMangement
                                   {
                                       SuiteID = suiteInfo.SUITE_ID,
                                       VehicleID = suiteInfo.VEHICLE_ID,
                                       CurrentStatus = suiteInfo.STATUS_WORKING,
                                       TempCurrentStatus = suiteInfo.STATUS,
                                       MdvrCoreId = suiteInfo.MDVR_CORE_SN,
                                       SuiteINFOID = suiteInfo.SUITE_INFO_ID,
                                   }).ToList();
                    System.Threading.Tasks.Parallel.ForEach(source, item =>
                    {
                        if (item.CurrentStatus == null)
                            item.CurrentStatus = item.TempCurrentStatus;
                    });
                    if (pageInfo.PageIndex > 0)
                    {
                        result.TotalRecord = source.Count();
                        result.Result = source.OrderBy(v => v.SuiteID).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                    }
                    else
                    {
                        result.TotalRecord = source.Count();
                        result.Result = source.ToList();
                    }
                }

                if (String.IsNullOrEmpty(suiteId))
                {
                    if (currentStatus == 22 || currentStatus == 23 || currentStatus == 24)
                    {
                        var source = from suiteInfo in context.STATUS_CHANGING_VIEW
                                     where suiteInfo.STATUS_WORKING == currentStatus
                                     select new InitialSuiteMangement
                                      {
                                          SuiteID = suiteInfo.SUITE_ID,
                                          VehicleID = suiteInfo.VEHICLE_ID,
                                          CurrentStatus = suiteInfo.STATUS_WORKING,
                                          MdvrCoreId = suiteInfo.MDVR_CORE_SN,
                                          SuiteINFOID = suiteInfo.SUITE_INFO_ID,
                                      };

                        if (pageInfo.PageIndex > 0)
                        {
                            result.TotalRecord = source.Count();
                            result.Result = source.OrderBy(v => v.SuiteID).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                        }
                        else
                        {
                            result.TotalRecord = source.Count();
                            result.Result = source.ToList();
                        }
                    }
                    if (currentStatus == 10 || currentStatus == 30 || currentStatus == 40)
                    {
                        var source = from suiteInfo in context.STATUS_CHANGING_VIEW
                                     where (short)suiteInfo.STATUS == currentStatus
                                     select new InitialSuiteMangement
                                      {
                                          SuiteID = suiteInfo.SUITE_ID,
                                          VehicleID = suiteInfo.VEHICLE_ID,
                                          CurrentStatus = (short)suiteInfo.STATUS,
                                          TempCurrentStatus = suiteInfo.STATUS,
                                          MdvrCoreId = suiteInfo.MDVR_CORE_SN,
                                          SuiteINFOID = suiteInfo.SUITE_INFO_ID,
                                      };

                        if (pageInfo.PageIndex > 0)
                        {
                            result.TotalRecord = source.Count();
                            result.Result = source.OrderBy(v => v.SuiteID).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                        }
                        else
                        {
                            result.TotalRecord = source.Count();
                            result.Result = source.ToList();
                        }

                    }
                }
                if (!String.IsNullOrEmpty(suiteId) && currentStatus != 0)
                {
                    if (currentStatus == 22 || currentStatus == 23 || currentStatus == 24)
                    {
                        var source = from suiteInfo in context.STATUS_CHANGING_VIEW
                                     where suiteInfo.STATUS_WORKING == currentStatus && suiteInfo.SUITE_ID.ToLower().Contains(suiteId.Trim().ToLower())
                                     select new InitialSuiteMangement
                                      {
                                          SuiteID = suiteInfo.SUITE_ID,
                                          VehicleID = suiteInfo.VEHICLE_ID,
                                          CurrentStatus = suiteInfo.STATUS_WORKING,
                                          MdvrCoreId = suiteInfo.MDVR_CORE_SN,
                                          SuiteINFOID = suiteInfo.SUITE_INFO_ID,
                                      };
                        if (pageInfo.PageIndex > 0)
                        {
                            result.TotalRecord = source.Count();
                            result.Result = source.OrderBy(v => v.SuiteID).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                        }
                        else
                        {
                            result.TotalRecord = source.Count();
                            result.Result = source.ToList();
                        }
                    }
                    if (currentStatus == 10 || currentStatus == 30 || currentStatus == 40)
                    {
                        var source = from suiteInfo in context.STATUS_CHANGING_VIEW
                                     where (short)suiteInfo.STATUS == currentStatus && suiteInfo.SUITE_ID.ToLower().Contains(suiteId.Trim().ToLower())
                                     select new InitialSuiteMangement
                                      {
                                          SuiteID = suiteInfo.SUITE_ID,
                                          VehicleID = suiteInfo.VEHICLE_ID,
                                          CurrentStatus = (short)suiteInfo.STATUS,
                                          //TempCurrentStatus = suiteInfo.STATUS,
                                          MdvrCoreId = suiteInfo.MDVR_CORE_SN,
                                          SuiteINFOID = suiteInfo.SUITE_INFO_ID,
                                      };
                        //System.Threading.Tasks.Parallel.ForEach(source, item =>
                        //{
                        //    if (item.CurrentStatus == null)
                        //        item.CurrentStatus = item.TempCurrentStatus;
                        //});      
                        if (pageInfo.PageIndex > 0)
                        {
                            result.TotalRecord = source.Count();
                            result.Result = source.OrderBy(v => v.SuiteID).Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
                        }
                        else
                        {
                            result.TotalRecord = source.Count();
                            result.Result = source.ToList();
                        }
                    }
                }

            }
            return result;

        }
        public bool RunningToAbnoraml(PTMSEntities context, SuiteMangementInfo newSuiteStatus)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadCommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                //var working = context.SECURITY_SUITE_WORKING.Where(x => x.VEHICLE_ID == newSuiteStatus.VehicleID).FirstOrDefault();
                //if (working != null)
                //{
                //    working.ABNORMAL_CAUSE = "99";
                //    working.FAULT_TIME = DateTime.Now;
                //    working.STATUS = newSuiteStatus.StatusChange;
                //    //newSuiteStatus.SuiteINFOID = working.SECURITY_SUITE_INFO.SUITE_INFO_ID;
                //}

                //context.SUITE_STATUS_CHANGE.Add(
                //    new SUITE_STATUS_CHANGE
                //  {
                //      ID = Guid.NewGuid().ToString(),
                //      SUITE_INFO_ID = newSuiteStatus.SuiteINFOID,
                //      OLD_STATUS = newSuiteStatus.CurrentStatus,
                //      NEW_STATUS = newSuiteStatus.StatusChange,
                //      OPERATING_TIME = DateTime.Now,
                //      OPERATING_PERSON = newSuiteStatus.UserInfo,
                //      CHANGE_REASON = newSuiteStatus.changeReason,
                //  });

                if (context.SaveChanges() > 0)
                {
                    scope.Complete();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }

        public bool AbnoramlToRepair(PTMSEntities context, SuiteMangementInfo newSuiteStatus)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadCommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                //var working = context.SECURITY_SUITE_WORKING.Where(x => x.VEHICLE_ID == newSuiteStatus.VehicleID).FirstOrDefault();
                //if (working != null)
                //{
                //    context.SECURITY_SUITE_WORKING.Remove(working);
                //}
                //var result = context.MONITOR_GROUP_VEHICLE.Where(x => x.VEHICLE_ID == newSuiteStatus.VehicleID).FirstOrDefault();
                //if (result != null)
                //{
                //    context.MONITOR_GROUP_VEHICLE.Remove(result);
                //}
                //var infosuite = context.DEV_SUITE.Where(x => x.SUITE_ID == newSuiteStatus.SuiteID).FirstOrDefault();
                //if (infosuite != null)
                //{
                //    infosuite.STATUS = newSuiteStatus.StatusChange;
                //    infosuite.CREATE_TIME = DateTime.Now;
                //}

                //var detail =
                //    context.INSTALLATION_DETAIL.Where(x => x.VEHICLE_ID == newSuiteStatus.VehicleID)
                //           .FirstOrDefault();
                //if (detail != null)
                //{
                //    var installaudit = context.INSTALLATION_AUDIT.FirstOrDefault(n => n.INSTALL_ID == detail.ID);
                //    if (installaudit != null)
                //    {
                //        context.INSTALLATION_AUDIT.Remove(installaudit);
                //    }
                //    context.INSTALLATION_DETAIL.Remove(detail);
                //}
                //var fence = context.VEHICLE_FENCE.Where(x => x.VEHICLE_ID == newSuiteStatus.VehicleID).ToList();
                //if (fence.Count != 0)
                //{
                //    System.Threading.Tasks.Parallel.ForEach(fence, item =>
                //    {
                //        context.VEHICLE_FENCE.Remove(item);
                //    });
                //}

                //var speedlimits = context.VEHICLE_SPEED.Where(x => x.VEHICLE_ID == newSuiteStatus.VehicleID).ToList();

                //if (speedlimits.Count != 0)
                //{
                //    System.Threading.Tasks.Parallel.ForEach(speedlimits, item =>
                //    {
                //        context.VEHICLE_SPEED.Remove(item);
                //    });
                //}

                //context.SUITE_STATUS_CHANGE.Add(
                //    new SUITE_STATUS_CHANGE
                //    {
                //        ID = Guid.NewGuid().ToString(),
                //        SUITE_INFO_ID = infosuite.SUITE_INFO_ID,
                //        OLD_STATUS = newSuiteStatus.CurrentStatus,
                //        NEW_STATUS = newSuiteStatus.StatusChange,
                //        OPERATING_TIME = DateTime.Now,
                //        OPERATING_PERSON = newSuiteStatus.UserInfo,
                //        CHANGE_REASON = newSuiteStatus.changeReason,
                //    });

                if (context.SaveChanges() > 0)
                {
                    scope.Complete();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }

        public bool AbnoramlToRunning(PTMSEntities context, SuiteMangementInfo newSuiteStatus)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadCommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                //var working = context.SECURITY_SUITE_WORKING.Where(x => x.VEHICLE_ID == newSuiteStatus.VehicleID).FirstOrDefault();

                //if (working != null)
                //{
                //    working.ABNORMAL_CAUSE = null;
                //    working.FAULT_TIME = null;
                //    working.STATUS = newSuiteStatus.StatusChange;
                //    //newSuiteStatus.SuiteINFOID = working.SECURITY_SUITE_INFO.SUITE_INFO_ID;
                //}

                //context.SUITE_STATUS_CHANGE.Add(
                //    new SUITE_STATUS_CHANGE
                //    {
                //        ID = Guid.NewGuid().ToString(),
                //        SUITE_INFO_ID = newSuiteStatus.SuiteINFOID,
                //        OLD_STATUS = newSuiteStatus.CurrentStatus,
                //        NEW_STATUS = newSuiteStatus.StatusChange,
                //        OPERATING_TIME = DateTime.Now,
                //        OPERATING_PERSON = newSuiteStatus.UserInfo,
                //        CHANGE_REASON = newSuiteStatus.changeReason,
                //    });
                if (context.SaveChanges() > 0)
                {
                    scope.Complete();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }

        public bool RepairToInitial(PTMSEntities context, SuiteMangementInfo newSuiteStatus)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadCommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                //var infosuite = context.SECURITY_SUITE_INFO.Where(x => x.SUITE_INFO_ID == newSuiteStatus.SuiteINFOID).FirstOrDefault();
                //if (infosuite != null)
                //{
                //    short s = 99;
                //    infosuite.STATUS = newSuiteStatus.StatusChange;// make the status of security_suite_info is 10
                //    newSuiteStatus.SuiteINFOID = infosuite.SUITE_INFO_ID;
                //    infosuite.CREATE_TIME = DateTime.Now;
                //    if (newSuiteStatus.StatusChange == 10)
                //    {
                //        context.SECURITY_SUITE_INFO.Add(
                //            new SECURITY_SUITE_INFO
                //           {
                //               SUITE_INFO_ID = Guid.NewGuid().ToString(),
                //               SUITE_ID = infosuite.SUITE_ID,
                //               MDVR_CORE_SN = infosuite.MDVR_CORE_SN,
                //               MDVR_SN = infosuite.MDVR_SN,
                //               MDVR_SIM = infosuite.MDVR_SIM,
                //               MDVR_SIM_MOBILE = infosuite.MDVR_SIM_MOBILE,
                //               DEVICE_TYPE = infosuite.DEVICE_TYPE,
                //               CAMERA_SN1 = infosuite.CAMERA_SN1,
                //               CAMERA_SN2 = infosuite.CAMERA_SN2,
                //               UPS_SN = infosuite.UPS_SN,
                //               SD_SN = infosuite.SD_SN,
                //               DOOR_SWITCH_SENSOR_SN = infosuite.DOOR_SWITCH_SENSOR_SN,
                //               SOFTWARE_VERSION = infosuite.SOFTWARE_VERSION,
                //               STATUS = s,
                //               CREATE_TIME = DateTime.Now,
                //               NOTE = infosuite.NOTE,
                //               CAMERA_SN3 = infosuite.CAMERA_SN3,
                //               CAMERA_SN4 = infosuite.CAMERA_SN4,
                //           });
                //    }
                //}
                //context.SUITE_STATUS_CHANGE.Add(
                //    new SUITE_STATUS_CHANGE
                //    {
                //        ID = Guid.NewGuid().ToString(),
                //        SUITE_INFO_ID = newSuiteStatus.SuiteINFOID,
                //        OLD_STATUS = newSuiteStatus.CurrentStatus,
                //        NEW_STATUS = newSuiteStatus.StatusChange,
                //        OPERATING_TIME = DateTime.Now,
                //        OPERATING_PERSON = newSuiteStatus.UserInfo,
                //        CHANGE_REASON = newSuiteStatus.changeReason,
                //    });
                if (context.SaveChanges() > 0)
                {
                    scope.Complete();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }

    }
}

