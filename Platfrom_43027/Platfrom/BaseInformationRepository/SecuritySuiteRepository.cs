/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ea0ee491-f94c-438b-9046-a9a21e4a2c45      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Repository
/////    Project Description:    
/////             Class Name: SecuritySuiteRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/20 16:42:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/20 16:42:00
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Message.Contract.Data;
using System.Linq.Expressions;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class SecuritySuiteRepository : Gsafety.PTMS.BaseInfo.BaseRepository
    {
        public SecuritySuiteRepository()
        {
        }

        /// <summary>
        /// Get Working SuiteInfo
        /// </summary>
        /// <param name="mdvrCoreSN"></param>
        /// <returns></returns>
        public List<WorkingSuiteInfo> GetWorkingSuiteInfo()
        {
            //using (PTMSEntities context = new PTMSEntities())
            //{
            //    var result = from si in context.DEV_SUITE
            //                 join sw in context.SECURITY_SUITE_WORKING on si.SUITE_INFO_ID equals sw.SUITE_INFO_ID
            //                 join v in context.VEHICLE on sw.VEHICLE_ID equals v.VEHICLE_ID
            //                 join c in context.DISTRICT on v.DISTRICT_CODE equals c.CODE
            //                 join p in context.DISTRICT on c.CODE.Substring(0, 2) equals p.CODE
            //                 where v.VALID == 1
            //                 && si.STATUS == 20 && sw.STATUS != 25
            //                 select new WorkingSuiteInfo
            //                 {
            //                     OrgnizationId = v.ORGNIZATION_ID,

            //                     VehicleId = v.VEHICLE_ID,
            //                     SuiteId = si.SUITE_ID,
            //                     DistrictCode = v.DISTRICT_CODE,
            //                     MdvrCoreId = si.MDVR_CORE_SN,
            //                     Status = (int)sw.STATUS,
            //                     SuiteInfoID = si.SUITE_INFO_ID,
            //                     CityCode = c.CODE,
            //                     ProvinceCode = p.CODE,
            //                     CityName = c.NAME,

            //                     ProvinceName = p.NAME,
            //                     VehicleType = (int)v.VEHICLE_TYPE,
            //                     BrandModel = v.BRAND_MODEL,
            //                     Owner = v.OWNER,
            //                     OperationLincese = v.OPERATION_LICENSE,
            //                     StartYear = v.START_YEAR,
            //                     VehicleSn = v.VEHICLE_SN,
            //                     Mobile = v.OWNER_PHONE,

            //                 };
            //    return result.ToList();
            //}
            return null;
        }

        /// <summary>
        /// get suite info using AlertManagement
        /// </summary>
        /// <returns></returns>
        public List<SuiteStatusInfo> GetWorkingSuiteInfoToAlertManager()
        {
            //using (PTMSEntities context = new PTMSEntities())
            //{
            //    var result = from si in context.DEV_SUITE
            //                 join sw in context.SECURITY_SUITE_WORKING on si.SUITE_INFO_ID equals sw.SUITE_INFO_ID
            //                 join v in context.VEHICLE on sw.VEHICLE_ID equals v.VEHICLE_ID
            //                 where v.VALID == 1
            //                 && si.STATUS == 20 && sw.STATUS != 25
            //                 select new SuiteStatusInfo
            //                 {
            //                     ClientId = v.CLIENT_ID,
            //                     VehicleId = v.VEHICLE_ID,
            //                     SuiteId = si.SUITE_ID,
            //                     DistrictCode = v.DISTRICT_CODE,
            //                     MdvrCoreId = si.MDVR_CORE_SN,
            //                     Status = (int)sw.STATUS,
            //                     SuiteInfoID = si.SUITE_INFO_ID,
            //                     VehicleType = (int)v.VEHICLE_TYPE,
            //                     OnlineFlag = sw.ONLINE_FLAG == 1 ? true : false,
            //                 };
            //    return result.ToList();
            //}
            return null;
        }

        /// <summary>
        /// get suite info by MdvrId
        /// </summary>
        /// <param name="mdvrid"></param>
        /// <returns></returns>
        public SuiteStatusInfo GetSuiteInfoToAlertManager(string mdvrid)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var result = from si in context.BSC_DEV_SUITE
                             join sw in context.RUN_SUITE_WORKING on si.SUITE_INFO_ID equals sw.SUITE_INFO_ID
                             join v in context.BSC_VEHICLE on sw.VEHICLE_ID equals v.VEHICLE_ID
                             where v.VALID == 1
                             && si.STATUS == 20 && sw.STATUS != 25
                             && sw.MDVR_CORE_SN == mdvrid
                             select new SuiteStatusInfo
                             {
                                 VehicleId = v.VEHICLE_ID,
                                 SuiteId = si.SUITE_ID,
                                 DistrictCode = v.DISTRICT_CODE,
                                 MdvrCoreId = si.MDVR_CORE_SN,
                                 Status = (int)sw.STATUS,
                                 SuiteInfoID = si.SUITE_INFO_ID,


                                 OnlineFlag = sw.ONLINE_FLAG == 1 ? true : false,
                             };
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// get suite info（messageconvert）
        /// </summary>
        /// <param name="mdvrCoreSN"></param>
        /// <returns></returns>
        public WorkingSuiteInfo GetSingleWorkingSuite(string mdvrCoreSN)
        {
            //using (PTMSEntities context = new PTMSEntities())
            //{
            //    var result = from si in context.DEV_SUITE
            //                 join sw in context.SECURITY_SUITE_WORKING on si.SUITE_INFO_ID equals sw.SUITE_INFO_ID
            //                 join v in context.VEHICLE on sw.VEHICLE_ID equals v.VEHICLE_ID

            //                 join c in context.DISTRICT on v.DISTRICT_CODE equals c.CODE
            //                 join p in context.DISTRICT on c.CODE.Substring(0, 2) equals p.CODE
            //                 where si.MDVR_CORE_SN == mdvrCoreSN
            //                 && v.VALID == 1
            //                 && si.STATUS == 20 && sw.STATUS != 25
            //                 select new WorkingSuiteInfo
            //                 {
            //                     VehicleId = v.VEHICLE_ID,
            //                     SuiteId = si.SUITE_ID,
            //                     DistrictCode = v.DISTRICT_CODE,
            //                     MdvrCoreId = si.MDVR_CORE_SN,
            //                     Status = (int)sw.STATUS,
            //                     SuiteInfoID = si.SUITE_INFO_ID,
            //                     CityCode = c.CODE,
            //                     ProvinceCode = p.CODE,
            //                     CityName = c.NAME,
            //                     ProvinceName = p.NAME,
            //                     VehicleType = (int)v.VEHICLE_TYPE,
            //                     BrandModel = v.BRAND_MODEL,
            //                     Owner = v.OWNER,
            //                     OperationLincese = v.OPERATION_LICENSE,
            //                     StartYear = v.START_YEAR,
            //                     VehicleSn = v.VEHICLE_SN,
            //                     Mobile = v.OWNER_PHONE,

            //                     ClientId = v.CLIENT_ID,
            //                     OrgnizationId = v.ORGNIZATION_ID,
            //                 };
            //    return result.FirstOrDefault();
            //}
            return null;
        }

        public bool UpdateSecuritySuite(DeviceSuite suite)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                if (context.MTN_INSTALLATION_DETAIL.Any(x => x.VALID == 1 && x.SUITE_INFO_ID == suite.Id))
                {
                    return false;
                }

                var result = context.BSC_DEV_SUITE.Where(x => x.SUITE_INFO_ID == suite.Id).FirstOrDefault();

                if (result != null)
                {
                    //result.ALARM_BUTTON_SN1 = suite.AlarmButton1Id;
                    //result.ALARM_BUTTON_SN2 = suite.AlarmButton2Id;
                    //result.ALARM_BUTTON_SN3 = suite.AlarmButton3Id;

                    //result.CAMERA_SN1 = suite.Camera1Id;
                    //result.CAMERA_SN2 = suite.Camera2Id;
                    //result.CAMERA_SN3 = suite.Camera3Id;
                    //result.CAMERA_SN4 = suite.Camera4Id;
                    //result.DEVICE_TYPE = (short)suite.DeviceType;
                    //result.DOOR_SWITCH_SENSOR_SN = suite.DoorSensorId;
                    result.MDVR_CORE_SN = suite.MdvrCoreId;
                    result.MDVR_SIM = suite.MdvrSimId;
                    result.MDVR_SIM_MOBILE = suite.MdvrSimPhoneNumber;
                    result.MDVR_SN = suite.MdvrId;
                    result.SD_SN = suite.SdCardId;
                    result.SOFTWARE_VERSION = suite.SoftwareVersion;
                    result.STATUS = (short)suite.status;
                    result.UPS_SN = suite.UpsId;
                    result.SUITE_ID = suite.SuiteId;
                    result.SUITE_INFO_ID = suite.Id;
                    result.NOTE = suite.Note;
                    context.SaveChanges();
                }
                return true;
            }
        }

        public bool DeleteSecuritySuite(DeviceSuite suite)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                if (context.MTN_INSTALLATION_DETAIL.Any(x => x.VALID == 1 && x.SUITE_INFO_ID == suite.Id))
                {
                    return false;
                }

                var result = context.BSC_DEV_SUITE.Where(x => x.SUITE_INFO_ID == suite.Id).FirstOrDefault();

                if (result != null)
                {
                    context.BSC_DEV_SUITE.Remove(result);
                    //result.VALID = 0;
                    context.SaveChanges();
                }
                return true;
            }
        }

        public bool AddSecuritySuite(DeviceSuite securitySuite)
        {
            //DEV_SUITE item = new DEV_SUITE();
            //item.ALARM_BUTTON_SN1 = securitySuite.AlarmButton1Id;
            //item.ALARM_BUTTON_SN2 = securitySuite.AlarmButton2Id;
            //item.ALARM_BUTTON_SN3 = securitySuite.AlarmButton3Id;

            //item.CAMERA_SN1 = securitySuite.Camera1Id;
            //item.CAMERA_SN2 = securitySuite.Camera2Id;
            //item.CAMERA_SN3 = securitySuite.Camera3Id;
            //item.CAMERA_SN4 = securitySuite.Camera4Id;
            //item.DEVICE_TYPE = (short)securitySuite.DeviceType;
            //item.DOOR_SWITCH_SENSOR_SN = securitySuite.DoorSensorId;
            //item.MDVR_CORE_SN = securitySuite.MdvrCoreId;
            //item.MDVR_SIM = securitySuite.MdvrSimId;
            //item.MDVR_SIM_MOBILE = securitySuite.MdvrSimPhoneNumber;
            //item.MDVR_SN = securitySuite.MdvrId;
            //item.SD_SN = securitySuite.SdCardId;
            //item.SOFTWARE_VERSION = securitySuite.SoftwareVersion;
            //item.STATUS = (short)DeviceSuiteStatus.Initial;
            //item.UPS_SN = securitySuite.UpsId;
            //item.SUITE_ID = securitySuite.SuiteId;
            //item.SUITE_INFO_ID = Guid.NewGuid().ToString();
            //item.NOTE = securitySuite.Note;
            ////INSTALLATION_DETAIL info = new INSTALLATION_DETAIL();
            ////info.ID = Guid.NewGuid().ToString();
            ////info.SUITE_INFO_ID = securitySuite.SuiteId;
            ////info.MDVR_CORE_SN = securitySuite.MdvrCoreId;         
            //using (PTMSEntities context = new PTMSEntities())
            //{
            //    context.SECURITY_SUITE_INFO.Add(item);
            //    context.SaveChanges();
            //    //context.INSTALLATION_DETAIL.Add(info);
            //    //context.SaveChanges();
            //    return true;
            //}
            return false;
        }

        public List<DeviceSuite> GetSecuritySuitesFuzzy(string vehicleId, string suiteId, string mdvrId, string mdvrCoreId, InstallStatusType? status, PagingInfo page, out int totalRecord, UserInfoMessageHeader header)
        {
            //using (PTMSEntities _context = new PTMSEntities())
            //{
            //var result = from x in _context.SECURITY_SUITE_INFO.Where(item => item.STATUS != (short)DeviceSuiteStatus.History)
            //             join y in _context.INSTALLATION_DETAIL.Where(item => item.VALID == 1) on x.SUITE_INFO_ID equals y.SUITE_INFO_ID 
            //    var result = from x in _context.SUITE_VIEW
            //                 //join setupStation in _context.SETUP_STATION.Where(item => item.VALID == 1) on y.STATION_ID equals setupStation.ID 
            //                 //from z in temp.DefaultIfEmpty()
            //                 where (string.IsNullOrEmpty(vehicleId) ? true : x.VEHICLE_ID.ToLower().Contains(vehicleId.ToLower())) &&
            //                       (string.IsNullOrEmpty(suiteId) ? true : x.SUITE_ID.Contains(suiteId)) &&
            //                       (string.IsNullOrEmpty(mdvrId) ? true : x.MDVR_SN.Contains(mdvrId)) &&
            //                       (string.IsNullOrEmpty(mdvrCoreId) ? true : x.MDVR_CORE_SN.Contains(mdvrCoreId)) &&
            //                       ((status != null && status == InstallStatusType.Installed) ? (x.CHECKSTEP == 7) : true) &&
            //                       ((status != null && status == InstallStatusType.Installing) ? (x.CHECKSTEP > 1 && x.CHECKSTEP < 7) : true) &&
            //                       ((status != null && status == InstallStatusType.UnInstall) ? (x.CHECKSTEP == 1 || x.CHECKSTEP == null) : true)
            //                       && x.STATUS != 99
            //                 select new DeviceSuite
            //                 {
            //                     AlarmButton1Id = x.ALARM_BUTTON_SN1,
            //                     AlarmButton2Id = x.ALARM_BUTTON_SN2,
            //                     AlarmButton3Id = x.ALARM_BUTTON_SN3,

            //                     Camera1Id = x.CAMERA_SN1,
            //                     Camera2Id = x.CAMERA_SN2,
            //                     Camera3Id = x.CAMERA_SN3,
            //                     Camera4Id = x.CAMERA_SN4,
            //                     DeviceType = x.DEVICE_TYPE == null ? VehicleType.Bus : (VehicleType)x.DEVICE_TYPE,
            //                     DoorSensorId = x.DOOR_SWITCH_SENSOR_SN,
            //                     Id = x.SUITE_INFO_ID,
            //                     MdvrCoreId = x.MDVR_CORE_SN,
            //                     MdvrId = x.MDVR_SN,
            //                     MdvrSimId = x.MDVR_SIM,
            //                     MdvrSimPhoneNumber = x.MDVR_SIM_MOBILE,
            //                     SdCardId = x.SD_SN,
            //                     SoftwareVersion = x.SOFTWARE_VERSION,
            //                     status = x.STATUS == null ? DeviceSuiteStatus.Initial : (DeviceSuiteStatus)x.STATUS,
            //                     SuiteId = x.SUITE_ID,
            //                     UpsId = x.UPS_SN,
            //                     Note = x.NOTE,
            //                     InstallStaff = x.INSTALL_STAFF,
            //                     VehicleId = x.VEHICLE_ID,
            //                     UpdateFlag = x.CHECKSTEP == null ? true : false,
            //                     //DeleteFlag = x.CHECKSTEP == null ? true : false,
            //                     CheckStep = x.CHECKSTEP,
            //                 };






            //    if (page == null || page.PageIndex == -1)
            //    {
            //        totalRecord = result.Count();
            //        List<DeviceSuite> Result = result.ToList();
            //        Result.ForEach(x =>
            //        {
            //            if (x.CheckStep == 7)
            //            {
            //                x.InstallStatus = InstallStatusType.Installed;
            //            }
            //            else if (x.CheckStep > 1)
            //            {
            //                x.InstallStatus = InstallStatusType.Installing;
            //            }
            //            else
            //            {
            //                x.InstallStatus = InstallStatusType.UnInstall;
            //            }
            //        });
            //        GetStatusWorking(Result);

            //        return Result;
            //    }
            //    else
            //    {
            //        totalRecord = result.Count();
            //        List<DeviceSuite> Result = result.OrderBy(x => x.Id).Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize).ToList();
            //        Result.ForEach(x =>
            //        {
            //            if (x.CheckStep == 7)
            //            {
            //                x.InstallStatus = InstallStatusType.Installed;
            //            }
            //            else if (x.CheckStep > 1)
            //            {
            //                x.InstallStatus = InstallStatusType.Installing;
            //            }
            //            else
            //            {
            //                x.InstallStatus = InstallStatusType.UnInstall;
            //            }
            //        });
            //        GetStatusWorking(Result);
            //        return Result;
            //    }
            //}
            totalRecord = 0;
            return null;
        }
        //Update  Bug 5122 ---xiay
        private void GetStatusWorking(List<DeviceSuite> Result)
        {
            using (PTMSEntities _context = new PTMSEntities())
            {
                //Result.ForEach(
                //    x =>
                //    {
                //        if (x.status == DeviceSuiteStatus.Working)
                //        {
                //            var statusinfo = from status in _context.STATUS_CHANGING_VIEW
                //                             where status.VEHICLE_ID == x.VehicleId
                //                             select status;
                //            if (statusinfo.FirstOrDefault() != null)
                //            {
                //                x.status = (DeviceSuiteStatus)statusinfo.FirstOrDefault().STATUS_WORKING;
                //            }
                //        }
                //    });
            }
        }



        public DeviceSuite GetSecuritySuiteBySuiteId(string suiteId, string mdvrid)
        {
            using (PTMSEntities _context = new PTMSEntities())
            {
                var result = (from x in _context.BSC_DEV_SUITE
                              where (string.IsNullOrEmpty(mdvrid) ? true : x.MDVR_SN == mdvrid) &&
                                    (string.IsNullOrEmpty(suiteId) ? true : x.SUITE_ID == suiteId)
                              select new DeviceSuite
                              {
                                  //AlarmButton1Id = x.ALARM_BUTTON_SN1,
                                  //AlarmButton2Id = x.ALARM_BUTTON_SN2,
                                  //AlarmButton3Id = x.ALARM_BUTTON_SN3,

                                  //Camera1Id = x.CAMERA_SN1,
                                  //Camera2Id = x.CAMERA_SN2,

                                  //Camera3Id = x.CAMERA_SN3,
                                  //Camera4Id = x.CAMERA_SN4,
                                  //DeviceType = x.DEVICE_TYPE == null ? VehicleType.Bus : (VehicleType)x.DEVICE_TYPE,
                                  //DoorSensorId = x.DOOR_SWITCH_SENSOR_SN,
                                  Id = x.SUITE_INFO_ID,
                                  MdvrCoreId = x.MDVR_CORE_SN,
                                  MdvrId = x.MDVR_SN,
                                  MdvrSimId = x.MDVR_SIM,
                                  MdvrSimPhoneNumber = x.MDVR_SIM_MOBILE,
                                  SdCardId = x.SD_SN,
                                  SoftwareVersion = x.SOFTWARE_VERSION,
                                  status = x.STATUS == null ? DeviceSuiteStatus.Initial : (DeviceSuiteStatus)x.STATUS,
                                  SuiteId = x.SUITE_ID,
                                  UpsId = x.UPS_SN,
                                  Note = x.NOTE,
                              }).OrderBy(c => c.status).FirstOrDefault();
                return result;
            }
        }

        public bool UpdateSecuritySuiteStatusByID(string Id, DeviceSuiteStatus status)
        {
            //using (PTMSEntities context = new PTMSEntities())
            //{
            //    var result = (from x in context.SECURITY_SUITE_INFO
            //                  where x.SUITE_INFO_ID == Id
            //                  select x).FirstOrDefault();
            //    if (result != null)
            //    {
            //        result.STATUS = (short)status;

            //        int i = context.SaveChanges();
            //        if (i > 0)
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            return false;
        }

        public bool CheckSecuritySuiteExistBySuiteID(string suiteID)
        {
            //using (PTMSEntities context = new PTMSEntities())
            //{
            //    var result = context.SECURITY_SUITE_INFO.Any(x => x.SUITE_ID == suiteID && x.STATUS != (short)DeviceSuiteStatus.History);
            //    return result;
            //}
            return false;
        }

        public bool CheckSecuritySuiteExistByMdvrCoreId(string mdvrCoreId)
        {
            //using (PTMSEntities context = new PTMSEntities())
            //{
            //    var result = context.SECURITY_SUITE_INFO.Any(x => x.MDVR_CORE_SN == mdvrCoreId && x.STATUS != (short)DeviceSuiteStatus.History);
            //    return result;
            //}
            return false;
        }

        public bool CheckSecuritySuiteExistByMdvrId(string mdvrId)
        {
            //using (PTMSEntities context = new PTMSEntities())
            //{
            //    var result = context.SECURITY_SUITE_INFO.Any(x => x.MDVR_SN == mdvrId && x.STATUS != (short)DeviceSuiteStatus.History);
            //    return result;
            //}
            return false;
        }

        public bool CheckInstallDetailById(string Id)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var result = (from x in context.MTN_INSTALLATION_DETAIL
                              where x.SUITE_INFO_ID == Id && x.VALID == 1
                              select x).ToList();
                if (result.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //public DeviceSuiteRepair GetRepairSuiteBySuiteid(string suiteid)
        //{
        //    using (PTMSEntities _context = new PTMSEntities())
        //    {
        //        DeviceSuiteRepair deviceSuiteRepair = new DeviceSuiteRepair();
        //        var result1 = (from x in _context.SECURITY_SUITE_INFO
        //                       where x.SUITE_ID == suiteid && (x.STATUS == 30 || x.STATUS == 99)
        //                       select new DeviceSuite
        //                       {
        //                           AlarmButton1Id = x.ALARM_BUTTON_SN1,
        //                           AlarmButton2Id = x.ALARM_BUTTON_SN2,
        //                           AlarmButton3Id = x.ALARM_BUTTON_SN3,

        //                           Camera1Id = x.CAMERA_SN1,
        //                           Camera2Id = x.CAMERA_SN2,
        //                           Camera3Id = x.CAMERA_SN3,
        //                           Camera4Id = x.CAMERA_SN4,
        //                           DeviceType = x.DEVICE_TYPE == null ? VehicleType.Bus : (VehicleType)x.DEVICE_TYPE,
        //                           DoorSensorId = x.DOOR_SWITCH_SENSOR_SN,
        //                           Id = x.SUITE_INFO_ID,
        //                           MdvrCoreId = x.MDVR_CORE_SN,
        //                           MdvrId = x.MDVR_SN,
        //                           MdvrSimId = x.MDVR_SIM,
        //                           MdvrSimPhoneNumber = x.MDVR_SIM_MOBILE,
        //                           SdCardId = x.SD_SN,
        //                           SoftwareVersion = x.SOFTWARE_VERSION,
        //                           status = x.STATUS == null ? DeviceSuiteStatus.Initial : (DeviceSuiteStatus)x.STATUS,
        //                           SuiteId = x.SUITE_ID,
        //                           UpsId = x.UPS_SN,
        //                           Note = x.NOTE,
        //                       }).FirstOrDefault();
        //        var result2 = (from x in _context.SECURITY_SUITE_INFO
        //                       where x.SUITE_ID == suiteid && (x.STATUS == 10 || x.STATUS == 20)
        //                       select new DeviceSuite
        //                       {
        //                           AlarmButton1Id = x.ALARM_BUTTON_SN1,
        //                           AlarmButton2Id = x.ALARM_BUTTON_SN2,
        //                           AlarmButton3Id = x.ALARM_BUTTON_SN3,

        //                           Camera1Id = x.CAMERA_SN1,
        //                           Camera2Id = x.CAMERA_SN2,
        //                           Camera3Id = x.CAMERA_SN3,
        //                           Camera4Id = x.CAMERA_SN4,
        //                           DeviceType = x.DEVICE_TYPE == null ? VehicleType.Bus : (VehicleType)x.DEVICE_TYPE,
        //                           DoorSensorId = x.DOOR_SWITCH_SENSOR_SN,
        //                           Id = x.SUITE_INFO_ID,
        //                           MdvrCoreId = x.MDVR_CORE_SN,
        //                           MdvrId = x.MDVR_SN,
        //                           MdvrSimId = x.MDVR_SIM,
        //                           MdvrSimPhoneNumber = x.MDVR_SIM_MOBILE,
        //                           SdCardId = x.SD_SN,
        //                           SoftwareVersion = x.SOFTWARE_VERSION,
        //                           status = x.STATUS == null ? DeviceSuiteStatus.Initial : (DeviceSuiteStatus)x.STATUS,
        //                           SuiteId = x.SUITE_ID,
        //                           UpsId = x.UPS_SN,
        //                           Note = x.NOTE,
        //                       }).FirstOrDefault();
        //        deviceSuiteRepair.OldDeviceSuite = result1;
        //        deviceSuiteRepair.NewDeviceSuite = result2;

        //        return deviceSuiteRepair;
        //    }
        //}

        public bool BatchAdd(List<DeviceSuite> suiteList)
        {
            //int errorIndex = -1;
            using (PTMSEntities context = new PTMSEntities())
            {
                for (int i = 0; i < suiteList.Count; i++)
                {
                    var item = suiteList[i];
                    //if (context.SECURITY_SUITE_INFO.Local.Any(x => (x.SUITE_ID == item.SuiteId || x.MDVR_SN == item.MdvrId || x.MDVR_CORE_SN == item.MdvrCoreId) && x.STATUS != (short)DeviceSuiteStatus.History))
                    //{
                    //    //errorIndex = i;
                    //    //break;
                    //    continue;
                    //}
                    //if (context.SECURITY_SUITE_INFO.Any(x => (x.SUITE_ID == item.SuiteId || x.MDVR_SN == item.MdvrId || x.MDVR_CORE_SN == item.MdvrCoreId) && x.STATUS != (short)DeviceSuiteStatus.History))
                    //{
                    //    //errorIndex = i;
                    //    //break;
                    //    continue;
                    //}
                    //context.SECURITY_SUITE_INFO.Add(new SECURITY_SUITE_INFO
                    //    {
                    //        ALARM_BUTTON_SN1 = item.AlarmButton1Id,
                    //        ALARM_BUTTON_SN2 = item.AlarmButton2Id,
                    //        ALARM_BUTTON_SN3 = item.AlarmButton3Id,

                    //        CAMERA_SN1 = item.Camera1Id,
                    //        CAMERA_SN2 = item.Camera2Id,
                    //        CAMERA_SN3 = item.Camera3Id,
                    //        CAMERA_SN4 = item.Camera4Id,
                    //        DEVICE_TYPE = (short)item.DeviceType,
                    //        DOOR_SWITCH_SENSOR_SN = item.DoorSensorId,
                    //        MDVR_CORE_SN = item.MdvrCoreId,
                    //        MDVR_SIM = item.MdvrSimId,
                    //        MDVR_SIM_MOBILE = item.MdvrSimPhoneNumber,
                    //        MDVR_SN = item.MdvrId,
                    //        SD_SN = item.SdCardId,
                    //        SOFTWARE_VERSION = item.SoftwareVersion,
                    //        STATUS = (short)DeviceSuiteStatus.Initial,
                    //        UPS_SN = item.UpsId,
                    //        SUITE_ID = item.SuiteId,
                    //        SUITE_INFO_ID = Guid.NewGuid().ToString(),
                    //        NOTE = item.Note,
                    //        CREATE_TIME = DateTime.Now,
                    //    });
                }
                context.SaveChanges();
                return true;
            }
        }

        public bool CheckSecuritySuiteExist(List<DeviceSuite> deviceSuiteList)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                //Func<DeviceSuite, bool> filter = tv => context.SECURITY_SUITE_INFO.Local.Any(v => v.STATUS != (short)DeviceSuiteStatus.History && (v.SUITE_ID == tv.SuiteId || v.MDVR_SN == tv.MdvrId || v.MDVR_CORE_SN == tv.MdvrCoreId))
                //     || context.SECURITY_SUITE_INFO.Any(v => v.STATUS != (short)DeviceSuiteStatus.History && (v.SUITE_ID == tv.SuiteId || v.MDVR_SN == tv.MdvrId || v.MDVR_CORE_SN == tv.MdvrCoreId));

                //if (deviceSuiteList.Any(filter))
                //{
                //    return true;
                //}
                //else
                //{
                return false;
                //}
            }
        }
    }
}
